# SearchBot

This bot sample talks about integrating C# v4 SDK bot service app with Bing search service.

## Details

-	Create bot service SDK app with "Core Bot" template.
-	Add nuget package [Microsoft.Azure.CognitiveServices.Search.WebSearch](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Search.WebSearch/) via package manager.
-	The [quickstart doc](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/web-search-sdk-quickstart) has good discussion how to work in client side for the Bing Search service.
-	Following is the core logic:
```diff
    // SearchAsync operation have various parameters exposed, which can be utilized for search service query filtering.
+    var webData = await _webSearchClient.Value.Web.SearchAsync(query: queryText, answerCount: 5);                
```
-	By looking at the source code for [WebSearch.Models.SearchResponse](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/cognitiveservices/Search.BingWebSearch/src/Generated/WebSearch/Models/SearchResponse.cs#L84), it could be inferred what are various details that can be pulled from the WebSearch service call.

### Note

It is just an example the way Search service could be called. It can always be enhanced with better modular design.
