using System.Collections.Generic;
using System.Linq;
using MegoTest.Models;
using MegoTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegoTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalSearchSystemsController : ControllerBase
    {
        /// <summary>
        /// Searches external search systems
        /// </summary>
        /// <param name="wait">Threshold time for a response from each systems. If the system did not respond in time, we are not waiting for its result.</param>
        /// <param name="randomMin">Minimum random range value</param>
        /// <param name="randomMax">Maximum random range value</param>
        /// <returns>List of search systems, request status and request duration for each system, total search duration</returns>
        [HttpGet("/search")]
        public ObjectResult Search(int wait = 2500, int randomMin = 100, int randomMax = 5000)
        {
            if (wait <= 0 || randomMin <= 0 || randomMax <= 0)
            {
                return Problem("Please, input correct params");
            }
            
            if (wait <= randomMin)
            {
                return Problem("[wait] param should be greater than [randomMin] param");
            }

            if (randomMin > randomMax)
            {
                return Problem("[randomMin] param should be less than [randomMax] param");
            }

            var searchResult = ExternalSearch.Search(wait, randomMin, randomMax);
            return Ok(searchResult);
        }

        /// <summary>
        /// Get metrics about search systems
        /// </summary>
        /// <param name="searchResults">List of SearchRequestResult entities</param>
        /// <returns>Time-based group metrics</returns>
        [HttpPost("/metrics")]
        public ObjectResult GetMetrics(List<SearchRequestResult> searchResults)
        {
            var metrics = MetricService.GroupByTime(searchResults);
            return Ok(metrics.ToList());
        }
    }
}