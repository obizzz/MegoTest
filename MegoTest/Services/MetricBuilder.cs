using System.Collections.Generic;
using System.Linq;
using MegoTest.Models;

namespace MegoTest.Services
{
    public class MetricBuilder : IMetricBuilder
    {
        public IEnumerable<Metric> GroupByTime(List<SearchRequestResult> searchResults)
        {
            var metrics = InitMetrics(searchResults.Max(e => e.Duration));
            foreach (var searchSystem in searchResults)
            {
                var group = searchSystem.Duration / 1000;
                metrics[group].RequestCount++;
                metrics[group].SearchSystemNames.Add(searchSystem.Name);
            }

            return metrics;
        }

        private Metric[] InitMetrics(long maxDuration)
        {
            var groupCount = maxDuration / 1000 + 1;
            var groups = new Metric[groupCount];

            for (int i = 0; i < groupCount; i++)
            {
                groups[i] = new Metric {TimeInterval = $"{i}..{i + 1}"};
            }

            return groups;
        }
    }
}