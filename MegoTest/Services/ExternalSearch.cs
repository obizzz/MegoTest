using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MegoTest.Models;

namespace MegoTest.Services
{
    public abstract class ExternalSearch
    {
        Random _random = new Random();

        public HttpStatusCode Request(int randomMin, int randomMax)
        {
            var duration = _random.Next(randomMin, randomMax);
            Thread.Sleep(duration);
            var success = _random.Next(1, 3) == 1;
            return success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        public async Task<HttpStatusCode> RequestAsync(int randomMin, int randomMax)
        {
            return await Task.Run(() => Request(randomMin, randomMax));
        }

        public static SearchTotalResult Search(int wait, int randomMin, int randomMax)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Stop();
            stopwatch.Restart();

            var searchTotalResult = new SearchTotalResult();

            var externalSearches = InitExternalSearches();
            searchTotalResult.SearchSystems = RequestAllAsync(externalSearches, wait, randomMin, randomMax);

            var externalC = searchTotalResult.SearchSystems.FirstOrDefault(x => x.Name == "ExternalC");
            if (externalC != null && externalC.Status == HttpStatusCode.OK.ToString())
            {
                var externalD = new ExternalD();
                var externalDResult =
                    RequestAllAsync(new List<ExternalSearch> {externalD}, wait, randomMin, randomMax);
                if (externalDResult.FirstOrDefault() != null)
                {
                    searchTotalResult.SearchSystems.Add(externalDResult.FirstOrDefault());
                }
            }

            searchTotalResult.SearchDuration = stopwatch.ElapsedMilliseconds;
            return searchTotalResult;
        }

        private static List<ExternalSearch> InitExternalSearches()
        {
            var externals = new List<ExternalSearch>();

            var externalA = new ExternalA();
            var externalB = new ExternalB();
            var externalC = new ExternalC();

            externals.Add(externalA);
            externals.Add(externalB);
            externals.Add(externalC);

            return externals;
        }

        private static List<SearchRequestResult> RequestAllAsync(List<ExternalSearch> externalSearches, int timeout,
            int randomMin, int randomMax)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Stop();
            stopwatch.Reset();

            var externals = new List<SearchRequest>();

            foreach (var externalSearch in externalSearches)
            {
                var searchRequest = new SearchRequest();
                searchRequest.ExternalSearch = externalSearch;
                searchRequest.Request = externalSearch.RequestAsync(randomMin, randomMax);
                externals.Add(searchRequest);
            }

            stopwatch.Start();

            var uncompletedRequestsCount = externals.Count;
            while (stopwatch.ElapsedMilliseconds < timeout && uncompletedRequestsCount > 0)
            {
                foreach (var external in externals)
                {
                    if (external.Request.IsCompleted && external.RequestDuration == -1)
                    {
                        external.RequestDuration = stopwatch.ElapsedMilliseconds;
                        uncompletedRequestsCount--;
                    }
                }
            }

            return GetResults(externals);
        }

        private static List<SearchRequestResult> GetResults(List<SearchRequest> requests)
        {
            var results = new List<SearchRequestResult>();

            foreach (var request in requests)
            {
                var searchResult = new SearchRequestResult();
                searchResult.Name = request.ExternalSearch.ToString()?.Split('.').LastOrDefault();
                if (request.Request.IsCompleted)
                {
                    searchResult.Status = request.Request.Result.ToString();
                    searchResult.Duration = request.RequestDuration;
                    results.Add(searchResult);
                }
            }

            return results;
        }
    }
}