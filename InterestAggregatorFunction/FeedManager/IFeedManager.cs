using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services
{
    public interface IFeedManager
    {
        public Dictionary<string, List<SyndicationItem>> FilterFeeds(Feed[] feeds);
    }
}
