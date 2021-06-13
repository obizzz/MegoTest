using System.Collections.Generic;

namespace MegoTest.Models
{
    public class SearchResult
    {
        public List<RequestResult> SearchSystems { get; set; }
        public long SearchDuration { get; set; }
    }
}