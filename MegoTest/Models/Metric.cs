using System.Collections.Generic;

namespace MegoTest.Models
{
    public class Metric
    {
        public string TimeInterval { get; set; }
        public List<string> SearchSystemNames { get; set; }
        public int RequestCount { get; set; }

        public Metric()
        {
            this.SearchSystemNames = new List<string>();
            this.RequestCount = 0;
        }
    }
}