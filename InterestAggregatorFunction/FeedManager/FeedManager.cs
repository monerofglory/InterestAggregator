using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.Filter;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace InterestAggregatorFunction.Services.FeedManager
{
    public class FeedManager : IFeedManager
    {
        public readonly IFeedFilter _feedFilter;
        public FeedManager(IFeedFilter feedFilter)
        {
            _feedFilter = feedFilter;
        }
        public Dictionary<string, List<SyndicationItem>> FilterFeeds(Feed[] feeds)
        {
            Dictionary<string, List<SyndicationItem>> resultDict = new();
            foreach (Feed feedObject in feeds)
            {
                List<SyndicationItem> filteredResult = GetFilteredFeed(feedObject);
                if (filteredResult.Count > 0)
                {
                    resultDict[feedObject.Name] = filteredResult;
                }
            }
            return resultDict;
        }

        private List<SyndicationItem> GetFilteredFeed(Feed feedObject)
        {
            string feed = GetRssXml(feedObject.GetUrl());
            SyndicationFeed syndicationFeed = GetSyndicationFeedFromXml(feed);
            return _feedFilter.FilterFeed(syndicationFeed, feedObject);
        }

        private static SyndicationFeed GetSyndicationFeedFromXml(string xml)
        {
            MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(xml));
            using XmlReader xmlReader = XmlReader.Create(memoryStream);
            return SyndicationFeed.Load(xmlReader);
        }

        private static string GetRssXml(string url)
        {
            Uri feedUri = new(url, UriKind.Absolute);
            HttpClient client = new(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, AllowAutoRedirect = true, MaxAutomaticRedirections = 3 });
            client.DefaultRequestHeaders.Add("accept", "text/html, application/xhtml+xml, */*");
            client.DefaultRequestHeaders.Add("user-agent", "Azure Function");
            return client.GetStringAsync(feedUri).Result;
        }
    }
}
