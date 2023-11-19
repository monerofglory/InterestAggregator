using Microsoft.Extensions.DependencyInjection;
using InterestAggregatorFunction;
using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services;
using System.Reflection;
using System.ServiceModel.Syndication;
using Xunit;
using FixtureFetchers;

namespace InterestAggregatorFunctionalTests
{
    public class EndToEndTest
    {
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly IHtmlContentBuilder _htmlContentBuilder;
        private readonly FixtureFetcher _fixtureFetcher = new();

        public EndToEndTest()
        {
            ServiceCollection services = new();

            IFeedConfig config = new FeedConfig
            {
                YamlPath = Assembly.GetExecutingAssembly().Location + "/../../../../../InterestAggregatorFunction/bin/Debug/net7.0/YamlFeedList.yml"
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
            var fixture = _fixtureFetcher.GetFixture("chelsea", DateOnly.FromDateTime(DateTime.Now.AddDays(1)));

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