using System.Collections.Generic;

namespace MegoTest.Models
{
    public class SearchTotalResult
    {
        public List<SearchRequestResult> SearchSystems { get; set; }
        public long SearchDuration { get; set; }
    }
}