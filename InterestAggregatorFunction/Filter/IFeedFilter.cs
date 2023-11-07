using InterestAggregatorFunction.FeedServices;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services.Filter
{
    public interface IFeedFilter
    {
        public List<SyndicationItem> FilterFeed(SyndicationFeed feed, Feed feedObj);
    }
}
