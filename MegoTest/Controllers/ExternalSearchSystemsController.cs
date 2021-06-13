using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using MegoTest.Models;
using MegoTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegoTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalSearchSystemsController : ControllerBase
    {
        [HttpGet("/search")]
        public SearchResult Search(int wait, int randomMin, int randomMax)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Stop();
            stopwatch.Restart();

            var searchResult = new SearchResult();
            
            var externalSearches = ExternalSearch.InitExternalSearches();
            searchResult.SearchSystems = ExternalSearch.RequestAllAsync(externalSearches, wait, randomMin, randomMax);

            var externalC = searchResult.SearchSystems.FirstOrDefault(x => x.Name == "ExternalC");
            if (externalC != null && externalC.Status == HttpStatusCode.OK.ToString())
            {
                var externalD = new ExternalD();
                var externalDResult = 
                    ExternalSearch.RequestAllAsync(new List<ExternalSearch> {externalD}, wait, randomMin, randomMax);
                searchResult.SearchSystems.Add(externalDResult.FirstOrDefault());
            }

            searchResult.SearchDuration = stopwatch.ElapsedMilliseconds;
            return searchResult;
        }
    }
}