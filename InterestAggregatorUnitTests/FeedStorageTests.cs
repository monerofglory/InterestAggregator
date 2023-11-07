using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services.FeedStorage;
using InterestAggregatorFunction.FeedServices;
using System.Reflection;

namespace InterestAggregatorUnitTests
{
    public class FeedStorageTests
    {
        private readonly FeedStorageYaml _sut;

        public FeedStorageTests()
        {
            IFeedConfig config = new FeedConfig
            {
                YamlPath = Assembly.GetExecutingAssembly().Location + "\\..\\..\\..\\..\\..\\InterestAggregatorFunction\\bin\\Debug\\net7.0\\YamlFeedList.yml"
            };
            _sut = new(config);
        }

        [Fact]
        public void Assert_FirstFeed_IsCK3()
        {
            //Act
            var allFeeds = _sut.GetFeeds();

            //Assert
            Assert.Equal("Crusader Kings", allFeeds.First().Name);
        }

        [Fact]
        public void Assert_FeedsReturn()
        {
            //Act
            var allFeeds = _sut.GetFeeds();

            //Assert
            Assert.NotNull(allFeeds);
            Assert.NotEmpty(allFeeds);
        }

        [Fact]
        public void Assert_WhenAllFeedsFetched_AtLeastOneReddit()
        {
            //Act
            var allFeeds = _sut.GetFeeds();

            var feeds = allFeeds.Where(x => x.FeedType == FeedTypeEnum.Reddit);

            //Assert
            Assert.NotNull(feeds);
            Assert.NotEmpty(feeds);
        }

        [Fact]
        public void Assert_WhenAllFeedsFetched_AtLeastOneSteam()
        {
            //Act
            var allFeeds = _sut.GetFeeds();

            var feeds = allFeeds.Where(x => x.FeedType == FeedTypeEnum.Steam);

            //Assert
            Assert.NotNull(feeds);
            Assert.NotEmpty(feeds);
        }
    }
}