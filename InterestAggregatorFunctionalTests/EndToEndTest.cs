using Microsoft.Extensions.DependencyInjection;
using InterestAggregatorFunction;
using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.IcsReader;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.FeedStorage;
using InterestAggregatorFunction.Services.HtmlGenerator;
using System.Reflection;
using System.ServiceModel.Syndication;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;

namespace InterestAggregatorFunctionalTests
{
    public class EndToEndTest
    {
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly HtmlGenerator _htmlGenerator;
        private readonly IcsReader _icsReader = new();

        public EndToEndTest()
        {
            ServiceCollection services = new();

            IFeedConfig config = new FeedConfig
            {
                YamlPath = Assembly.GetExecutingAssembly().Location + "\\..\\..\\..\\..\\InterestAggregatorFunction\\bin\\Debug\\net7.0\\YamlFeedList.yml"
            };
            FeedStorageYaml feedStorageYaml = new(config);

            services = Runner.RegisterDependencies(services, feedStorageYaml);
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _feedManager = serviceProvider.GetService<IFeedManager>();
            _feedStorage = serviceProvider.GetService<IFeedStorage>();
            //_htmlGenerator = serviceProvider.GetService<IHtmlGenerator>();
            _htmlGenerator = new HtmlGenerator();
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
            string emailBody = _htmlGenerator.Begin()
                .ConstructFeedHtml(filteredFeeds)
                .ConstructOtherHtml(fixtureName, kickoff.ToShortTimeString())
                .End();

            //Send the email
            Assert.NotEmpty(emailBody);
        }
    }
}