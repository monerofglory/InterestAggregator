using InterestAggregatorFunction.FeedServices;

namespace InterestAggregatorFunction.Services.FeedStorage
{
    public interface IFeedStorage
    {
        public Feed[] GetFeeds();
    }
}
