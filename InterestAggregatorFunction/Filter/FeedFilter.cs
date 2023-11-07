using InterestAggregatorFunction.FeedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services.Filter
{
    public class FeedFilter : IFeedFilter
    {
        public List<SyndicationItem> FilterFeed(SyndicationFeed feed, Feed feedObj) =>
            feedObj.FeedType switch
            {
                FeedTypeEnum.Reddit => FilterBasedOn(feed, feedObj.TitleWords, feedObj.UserWords),
                FeedTypeEnum.Steam => FilterBasedOn(feed, feedObj.TitleWords, feedObj.UserWords),
                _ => throw new ArgumentException("Unsupported feed type"),
            };

        private static List<SyndicationItem> FilterBasedOn(SyndicationFeed feed, List<string> titleWords = null, List<string> userWords = null)
        {
            //Filter out old items
            var items = feed.Items.Where(item => item.PublishDate >= DateTime.Now.AddDays(-1));

            if (titleWords != null && titleWords.Count != 0)
            {
                items = TitleWordsFilter(items, titleWords);
            }

            if (userWords != null && userWords.Count != 0)
            {
                items = UserWordsFilter(items, userWords);
            }

            return items.ToList();
        }

        private static IEnumerable<SyndicationItem> TitleWordsFilter(IEnumerable<SyndicationItem> items, List<string> titleWords)
        {
            return items.Where(item => titleWords.Any(title => item.Title.Text.Contains(title)));
        }

        private static IEnumerable<SyndicationItem> UserWordsFilter(IEnumerable<SyndicationItem> items, List<string> userWords)
        {
            return items.Where(item => item.Authors.Any(author => userWords.Contains(author.Name)));
        }
    }
}
