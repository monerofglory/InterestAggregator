using InterestAggregatorFunction.Services.HtmlContentBuilder;
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

            string fixtureName = "Chelsea vs Liverpool";
            string fixtureTime = "12:30pm";
            //Act
            var emailBody = _sut
                .WithRssFeedContent(syndDict)
                .WithFixtureContent(fixtureName, fixtureTime)
                .Build();

            //Assert
            Assert.Equal("<PRE><b>Test Game Title</b>\n<a href=\"http://www.example.com/\" target=\"blank\">Test Title</a>\n\n<b>Chelsea vs Liverpool\n</b>12:30pm\n</PRE>", emailBody);
        }

        [Fact]
        public void Assert_WhenNoFeeds_AndNoFixture_CorrectBody()
        {
            //Arrange
            var emptyDict = new Dictionary<string, List<SyndicationItem>>();

            //Act
            var emailBody = _sut
                .WithRssFeedContent(emptyDict)
                .WithFixtureContent(string.Empty, "N/A")
                .Build();

            //Assert
            Assert.Equal("No news today.", emailBody);
        }
    }
}