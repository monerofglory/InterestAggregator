using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.EmailManager;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.FeedStorage;
using InterestAggregatorFunction.Services.HtmlGenerator;
using InterestAggregatorFunction.Services.IcsReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace InterestAggregatorFunction
{
    public class EveningEmail
    {
        private readonly IEmailManager _emailManager;
        private readonly IFeedManager _feedManager;
        private readonly IFeedStorage _feedStorage;
        private readonly HtmlGenerator _htmlGenerator;
        private readonly IcsReader _icsReader = new();

        public EveningEmail(IEmailManager emailManager, IFeedManager feedManager, IFeedStorage feedStorage)
        {
            _emailManager = emailManager;
            _feedManager = feedManager;
            _feedStorage = feedStorage;
            _htmlGenerator = new HtmlGenerator();
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
            string htmlFeedBody = _htmlGenerator
                .Begin()
                .ConstructFeedHtml(filteredFeeds)
                .ConstructOtherHtml(fixtureName, kickoff.ToShortTimeString())
                .End();

            //Send the html as email
            _emailManager.SendEmail(htmlFeedBody);
        }
    }
}
