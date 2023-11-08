namespace InterestAggregatorFunction.Services
{
    public class Feed
    {
        public string Name { get; set; }
        public FeedTypeEnum FeedType { get; set; }
        public string Link { get; set; }
        public List<string> TitleWords { get; set; }
        public List<string> UserWords { get; set; }

        public Feed(string name, FeedTypeEnum feedType, string link, List<string>? titleWords = null, List<string>? userWords = null)
        {
            Name = name;
            FeedType = feedType;
            Link = link;
            TitleWords = titleWords ?? new List<string>();
            UserWords = userWords ?? new List<string>();
        }

        public Feed() { } //A parameter-less constructor is required for deserialization from YAML.

        public string GetUrl() =>
            FeedType switch
            {
                FeedTypeEnum.Reddit => $"https://www.reddit.com/r/{Link}/.rss",
                FeedTypeEnum.Steam => $"https://store.steampowered.com/feeds/news/app/{Link}",
                _ => throw new ArgumentException("Unsupported feed type")
            };
    }
    public enum FeedTypeEnum
    {
        Steam,
        Reddit
    }
}
