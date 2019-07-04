using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchBot.Models
{
    [Serializable]
    public class SearchPage
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string DisplayUrl { get; set; }
    }
}
