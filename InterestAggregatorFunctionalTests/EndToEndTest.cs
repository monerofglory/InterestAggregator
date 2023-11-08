using Microsoft.Extensions.DependencyInjection;
using InterestAggregatorFunction;
using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.IcsReader;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.FeedStorage;
using System.Reflection;
using System.ServiceModel.Syndication;
using Xunit;
using InterestAggregatorFunction.Services.HtmlContentBuilder;

namespace InterestAggregatorFunctionalTests
{
    public class EndToEndTest
    {
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly IHtmlContentBuilder _htmlContentBuilder;
        private readonly IcsReader _icsReader = new();

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
            var (fixtureName, kickoff) = _icsReader.CheckFootballFixtures("chelsea");

            //Construct the email
            string emailBody = _htmlContentBuilder
                .WithRssFeedContent(filteredFeeds)
                .WithFixtureContent(fixtureName, kickoff.ToShortTimeString())
                .Build();

            //Send the email
            Assert.NotEmpty(emailBody);
        }
    }
}