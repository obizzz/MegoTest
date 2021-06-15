using System.Net;
using System.Threading.Tasks;
using MegoTest.Models;

namespace MegoTest.Services
{
    public interface IExternalSearch
    {
        public HttpStatusCode Request(int randomMin, int randomMax);
        public Task<HttpStatusCode> RequestAsync(int randomMin, int randomMax);
        public SearchTotalResult Search(int wait, int randomMin, int randomMax);
    }
}