using System.Net;
using System.Threading.Tasks;
using MegoTest.Services;

namespace MegoTest.Models
{
    public class SearchRequest
    {
        public ExternalSearch ExternalSearch { get; set; }
        public Task<HttpStatusCode> Request { get; set; }
        public long RequestDuration { get; set; } = -1;
    }
}