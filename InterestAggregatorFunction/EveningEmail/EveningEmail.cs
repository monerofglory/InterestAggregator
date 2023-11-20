using FixtureFetchers;
using InterestAggregatorFunction.Services;
using System.ServiceModel.Syndication;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

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
            var fixture = FixtureFetcher.GetFixture("chelsea", DateOnly.FromDateTime(DateTime.Now.AddDays(1))) 
                ?? FixtureFetcher.GetFixture("england", DateOnly.FromDateTime(DateTime.Now.AddDays(1)));

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
