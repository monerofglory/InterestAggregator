using InterestAggregatorFunction.FeedServices;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services.FeedManager
{
    public interface IFeedManager
    {
        public Dictionary<string, List<SyndicationItem>> FilterFeeds(Feed[] feeds);
    }
}
