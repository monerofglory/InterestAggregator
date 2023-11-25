using InterestAggregatorFunction.ServiceDtos;
using InterestAggregatorFunction.Services;
using System.Net;
using System.Net.Http.Json;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction
{
    public class EveningEmail
    {
        private readonly IEmailManager _emailManager;
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly IHtmlContentBuilder _htmlContentBuilder;

        public EveningEmail(IEmailManager emailManager, IFeedManager feedManager, IFeedStorage feedStorage, IHtmlContentBuilder htmlContentBuilder)
        {
            _emailManager = emailManager;
            _feedManager = feedManager;
            _feedStorage = feedStorage;
            _htmlContentBuilder = htmlContentBuilder;
        }

        public void Run()
        {
            //Get the feeds
            Feed[] feeds = _feedStorage.GetFeeds();

            //Filter the feeds
            Dictionary<string, List<SyndicationItem>> filteredFeeds = _feedManager.FilterFeeds(feeds);

            //Fetch football fixtures.
            Fixture fixture;
            var fixtureServiceResult = new HttpClient().GetAsync("https://fixturefetcherservice.azurewebsites.net/fixturefetcher/gettomorrowsfixture/chelsea").Result;
            if (fixtureServiceResult.StatusCode == HttpStatusCode.NotFound)
            {
                fixture = null;
            }
            else
            {
                fixture = fixtureServiceResult.Content.ReadFromJsonAsync<Fixture>().Result;
            }

            //Construct the htmlBody
            string htmlFeedBody = _htmlContentBuilder
                .WithRssFeedContent(filteredFeeds)
                .WithFixtureContent(fixture)
                .Build();

            //Send the html as email
            _emailManager.SendEmail(htmlFeedBody);
        }
    }
}
