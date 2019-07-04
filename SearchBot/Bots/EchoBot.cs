// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Newtonsoft.Json;
using SearchBot.Models;

namespace SearchBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        // Lazy initialization of private variables is to adopt Singleton pattern for objects like HttpClient, WebSearchClient.
        private static ApiKeyServiceClientCredentials apiKeyServiceClientCredentials;
        private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() => new HttpClient());
        private static readonly Lazy<WebSearchClient> _webSearchClient = new Lazy<WebSearchClient>(() => new WebSearchClient(apiKeyServiceClientCredentials, _httpClient.Value, false));
        
        public EchoBot(IConfiguration configuration)
        {
            // Read BingSearch service key with the help of DI technique, as it's already been loaded in middleware configuration (in Startup).
            apiKeyServiceClientCredentials = new ApiKeyServiceClientCredentials(configuration["BingSearchKey"]);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await SearchPagesInAttachment(turnContext.Activity.Text);
            
            // text response
            // await turnContext.SendActivityAsync(MessageFactory.Text($"{JsonConvert.SerializeObject(results)}"), cancellationToken);

            // carousel card response
            await turnContext.SendActivityAsync(MessageFactory.Carousel(results), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and Welcome!"), cancellationToken);
                }
            }
        }

        private async Task<IList<Attachment>> SearchPagesInAttachment(string queryText)
        {
            var attachmentResults = await SearchPages(queryText);
            return attachmentResults.Select(s => new Attachment
            {
                Name = s.Name,
                ContentUrl = s.Url,
                ThumbnailUrl = s.DisplayUrl
            }).ToList();
        }

        private async Task<IList<SearchPage>> SearchPages(string queryText) {
            IList<SearchPage> webResults = new List<SearchPage>();

            try
            {
                // SearchAsync operation have various parameters exposed, which can be utilized for search service query filtering.
                var webData = await _webSearchClient.Value.Web.SearchAsync(query: queryText, answerCount: 5);                

                if (webData?.WebPages?.Value?.Count > 0)
                {
                    webResults = webData.WebPages.Value.Select(s =>
                                                                new SearchPage
                                                                {
                                                                    Name = s.Name,
                                                                    Url = s.Url,
                                                                    DisplayUrl = s.ThumbnailUrl
                                                                }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return webResults;
        }
    }
}
