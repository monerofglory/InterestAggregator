using Moq;
using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.Filter;

namespace InterestAggregatorUnitTests
{
    public class FeedManagerTests
    {
        [Fact]
        public void Assert_WhenNoFeeds_EmptyDict()
        {
            //Arrange
            var feeds = Array.Empty<Feed>();
            var sut = new FeedManager(new Mock<IFeedFilter>().Object);

            //Act
            var filteredFeed = sut.FilterFeeds(feeds);

            //Assert
            Assert.Empty(filteredFeed);
        }
    }
}