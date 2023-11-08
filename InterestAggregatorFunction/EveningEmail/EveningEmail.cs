using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.EmailManager;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.FeedStorage;
using InterestAggregatorFunction.Services.HtmlContentBuilder;
using InterestAggregatorFunction.Services.IcsReader;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction
{
    public class EveningEmail
    {
        private readonly IEmailManager _emailManager;
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly IHtmlContentBuilder _htmlContentBuilder;
        private readonly IcsReader _icsReader = new();

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

            //Fetch football fixtures
            var (fixtureName, kickoff) = _icsReader.CheckFootballFixtures("chelsea");

            //Construct the htmlBody
            string htmlFeedBody = _htmlContentBuilder
                .WithRssFeedContent(filteredFeeds)
                .WithFixtureContent(fixtureName, kickoff.ToShortTimeString())
                .Build();

            //Send the html as email
            _emailManager.SendEmail(htmlFeedBody);
        }
    }
}
