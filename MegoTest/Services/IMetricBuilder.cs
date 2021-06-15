using System.Collections.Generic;
using MegoTest.Models;

namespace MegoTest.Services
{
    public interface IMetricBuilder
    {
        public IEnumerable<Metric> GroupByTime(List<SearchRequestResult> searchResults);
    }
}