using FixtureFetchers;
using System.ServiceModel.Syndication;

namespace InterestAggregatorUnitTests
{
    public class HtmlContentBuilderTests
    {
        private readonly HtmlContentBuilder _sut = new();
        [Fact]
        public void Assert_WhenNoFeeds_CorrectBody()
        {
            //Arrange
            var emptyDict = new Dictionary<string, List<SyndicationItem>>();

            //Act
            var emailBody = _sut
                .WithRssFeedContent(emptyDict)
                .Build();

            //Assert
            Assert.Equal("No news today.", emailBody);
        }

        [Fact]
        public void Assert_WhenOneFeed_CorrectBody()
        {
            //Arrange

            List<SyndicationItem> syndicationItems = new() {
                new SyndicationItem("Test Title", "Test Content", new Uri("http://www.example.com"), "TestId", DateTimeOffset.Now)
            };

            var syndDict = new Dictionary<string, List<SyndicationItem>>
            {
                ["Test Game Title"] = syndicationItems
            };

            //Act
            var emailBody = _sut
                .WithRssFeedContent(syndDict)
                .Build();

            //Assert
            Assert.Equal("<PRE><b>Test Game Title</b>\n<a href=\"http://www.example.com/\" target=\"blank\">Test Title</a>\n\n</PRE>", emailBody);
        }

        [Fact]
        public void Assert_WhenOneFeed_AndOneFixtureCorrectBody()
        {
            //Arrange

            List<SyndicationItem> syndicationItems = new() {
                new SyndicationItem("Test Title", "Test Content", new Uri("http://www.example.com"), "TestId", DateTimeOffset.Now)
            };

            var syndDict = new Dictionary<string, List<SyndicationItem>>
            {
                ["Test Game Title"] = syndicationItems
            };

            var fixture = new Fixture("Chelsea vs Liverpool", new DateTime(2023, 11, 23, 14, 30, 0));
            //Act
            var emailBody = _sut
                .WithRssFeedContent(syndDict)
                .WithFixtureContent(fixture)
                .Build();

            //Assert
            Assert.Equal("<PRE><b>Test Game Title</b>\n<a href=\"http://www.example.com/\" target=\"blank\">Test Title</a>\n\n<b>Chelsea vs Liverpool\n</b>14:30\n</PRE>", emailBody);
        }

        [Fact]
        public void Assert_WhenNoFeeds_AndNoFixture_CorrectBody()
        {
            //Arrange
            var emptyDict = new Dictionary<string, List<SyndicationItem>>();

            Fixture fixture = null;

            //Act
            var emailBody = _sut
                .WithRssFeedContent(emptyDict)
                .WithFixtureContent(fixture)
                .Build();

            //Assert
            Assert.Equal("No news today.", emailBody);
        }
    }
}