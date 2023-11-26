using InterestAggregatorFunction.ServiceDtos;
using System.Net.Http.Json;
using System.Net;

namespace InterestAggregatorFunction.Services
{
    public static class FixtureFetcher
    {
        public static Fixture? GetTomorrowsFixture(string team)
        {
            HttpResponseMessage? fixtureServiceResult = new HttpClient().Send(new HttpRequestMessage(HttpMethod.Get, $"https://fixturefetcherservice.azurewebsites.net/fixturefetcher/gettomorrowsfixture/{team}"));
            if (fixtureServiceResult.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                return fixtureServiceResult.Content.ReadFromJsonAsync<Fixture>().Result;
            }
        }
    }
}
