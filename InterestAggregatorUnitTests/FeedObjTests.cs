using InterestAggregatorFunction.FeedServices;

namespace InterestAggregatorUnitTests
{
    public class FeedObjTests
    {
        [Fact]
        public void Assert_GetLinkIsCorrect_WhenSteam()
        {
            //Arrange
            Feed testFeedObj = new("Test", FeedTypeEnum.Steam, "11010011", null, null);

            //Act
            string testLink = testFeedObj.GetUrl();

            //Assert
            Assert.Equal("https://store.steampowered.com/feeds/news/app/11010011", testLink);
        }

        [Fact]
        public void Assert_GetLinkIsCorrect_WhenReddit()
        {
            //Arrange
            Feed testFeedObj = new("Test", FeedTypeEnum.Reddit, "11010011", null, null);

            //Act
            string testLink = testFeedObj.GetUrl();

            //Assert
            Assert.Equal("https://www.reddit.com/r/11010011/.rss", testLink);
        }
    }
}