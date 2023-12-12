using System.ServiceModel.Syndication;

namespace InterestAggregatorUnitTests
{
    public class FeedFilterTests
    {
        private readonly FeedFilter _sut = new();
        private readonly SyndicationFeed _syndicationItems;

        public FeedFilterTests()
        {
            _syndicationItems = GetSampleListOfSyndicationItems();
        }

        [Fact]
        public void Assert_WhenValidTitleWord_FiltersCorrectly()
        {
            //Arrange
            SyndicationItem expectedReturn = _syndicationItems.Items.First(); //A Whole New World

            List<string> titleFilters = ["World"];
            Feed feedObj = new("Movie Subreddit", FeedTypeEnum.Reddit, "www.example.com/r/movies", titleFilters);

            //Act
            List<SyndicationItem> filteredFeed = _sut.FilterFeed(_syndicationItems, feedObj);

            //Assert
            Assert.Equal(expectedReturn, filteredFeed.First());
            Assert.Single(filteredFeed);
        }

        [Fact]
        public void Assert_WhenValidTitleWords_FiltersCorrectly()
        {
            //Arrange
            List<string> titleFilters = ["World", "Bad"];
            Feed feedObj = new("Movie Subreddit", FeedTypeEnum.Reddit, "www.example.com/r/movies", titleFilters);

            //Act
            List<SyndicationItem> filteredFeed = _sut.FilterFeed(_syndicationItems, feedObj);

            //Assert
            Assert.Equal(2, filteredFeed.Count);
        }

        [Fact]
        public void Assert_WhenNoTitleWords_ReturnsEverything()
        {
            //Arrange
            List<string> titleFilters = [];
            Feed feedObj = new("Movie Subreddit", FeedTypeEnum.Reddit, "www.example.com/r/movies", titleFilters);

            //Act
            List<SyndicationItem> filteredFeed = _sut.FilterFeed(_syndicationItems, feedObj);

            //Assert
            Assert.Equal(3, filteredFeed.Count);
        }

        [Fact]
        public void Assert_WhenUnsupportedFeedType_ThrowsArgumentException()
        {
            //Arrange
            List<string> titleFilters = [];
            Feed feedObj = new("Movie Subreddit", (FeedTypeEnum)99, "www.example.com/r/movies", titleFilters);

            //Act and Assert
            Assert.Throws<ArgumentException>(() => _sut.FilterFeed(_syndicationItems, feedObj));
        }

        private static SyndicationFeed GetSampleListOfSyndicationItems()
        {
            List<SyndicationItem> syndicationItemList = [];

            SyndicationItem item1 = new("A Whole New World", "Watch the new movie by Grisney!", new Uri("http://www.example.com"));
            item1.PublishDate = DateTime.Now;
            syndicationItemList.Add(item1);

            SyndicationItem item2 = new("The Bad Gatsby", "This is truly cinematic!", new Uri("http://www.example.com"));
            item2.PublishDate = DateTime.Now;
            syndicationItemList.Add(item2);

            SyndicationItem item3 = new("A Different World", "Quite rubbish!", new Uri("http://www.example.com"));
            item3.PublishDate = DateTime.Now.AddDays(-1);
            syndicationItemList.Add(item3);

            SyndicationItem item4 = new("The Least Favourite", "Worthy of a king!", new Uri("http://www.example.com"));
            item4.PublishDate = DateTime.Now;
            syndicationItemList.Add(item4);

            return new SyndicationFeed(syndicationItemList);
        }
    }
}
