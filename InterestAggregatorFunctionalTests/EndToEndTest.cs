using Microsoft.Extensions.DependencyInjection;
using InterestAggregatorFunction;
using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services;
using InterestAggregatorFunction.ServiceDtos;
using System.Reflection;
using System.ServiceModel.Syndication;
using Xunit;

namespace InterestAggregatorFunctionalTests
{
    public class EndToEndTest
    {
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly IHtmlContentBuilder _htmlContentBuilder;

        public EndToEndTest()
        {
            ServiceCollection services = new();

            IFeedConfig config = new FeedConfig
            {
                YamlPath = Assembly.GetExecutingAssembly().Location + "/../../../../../InterestAggregatorFunction/YamlFeedList.yml"
            };
            FeedStorageYaml feedStorageYaml = new(config);

            services = Runner.RegisterDependencies(services, feedStorageYaml);
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _feedManager = serviceProvider.GetRequiredService<IFeedManager>();
            _feedStorage = serviceProvider.GetRequiredService<IFeedStorage>();
            _htmlContentBuilder = serviceProvider.GetRequiredService<IHtmlContentBuilder>();
        }

        [Fact]
        public void EndToEnd_NoCrashes()
        {
            //Get the feeds
            Feed[] feeds = _feedStorage.GetFeeds();

            //Filter the feeds
            Dictionary<string, List<SyndicationItem>> filteredFeeds = _feedManager.FilterFeeds(feeds);

            //Fetch football fixtures
            Fixture? fixture = FixtureFetcher.GetTomorrowsFixture("chelsea");

            //Construct the email
            string emailBody = _htmlContentBuilder
                .WithRssFeedContent(filteredFeeds)
                .WithFixtureContent(fixture)
                .Build();

            //Send the email
            Assert.NotEmpty(emailBody);
        }
    }
}