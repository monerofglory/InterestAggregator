using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services
{
    public interface IFeedFilter
    {
        public List<SyndicationItem> FilterFeed(SyndicationFeed feed, Feed feedObj);
    }
}
