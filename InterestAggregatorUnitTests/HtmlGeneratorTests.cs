using InterestAggregatorFunction.Services.HtmlGenerator;
using System.ServiceModel.Syndication;

namespace InterestAggregatorUnitTests
{
    public class HtmlGeneratorTests
    {
        private readonly HtmlGenerator _sut = new();
        [Fact]
        public void Assert_WhenNoFeeds_CorrectBody()
        {
            //Arrange
            var emptyDict = new Dictionary<string, List<SyndicationItem>>();

            //Act
            var emailBody = _sut
                .Begin()
                .ConstructFeedHtml(emptyDict)
                .End();

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
                .Begin()
                .ConstructFeedHtml(syndDict)
                .End();

            //Assert
            Assert.Equal("<PRE><b>Test Game Title</b>\n<a href=\"http://www.example.com/\" target=\"blank\">Test Title</a>\n\n</PRE>", emailBody);
        }
    }
}