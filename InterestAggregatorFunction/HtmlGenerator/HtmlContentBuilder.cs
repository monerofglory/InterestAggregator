using InterestAggregatorFunction.ServiceDtos;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services
{
    public class HtmlContentBuilder : IHtmlContentBuilder
    {
        private string? _feedHtml = null;
        private string? _fixtureHtml = null;

        public string Build()
        {
            string returnHtml = "<PRE>";
            
            if (_fixtureHtml == null & _feedHtml == null)
            {
                return "No news today.";
            }

            if (_feedHtml != null)
            {
                returnHtml += _feedHtml;
            }

            if (_fixtureHtml != null)
            {
                returnHtml += _fixtureHtml;
            }

            returnHtml += "</PRE>";
            return returnHtml;
        }

        public IHtmlContentBuilder WithFixtureContent(Fixture fixture)
        {
            if (fixture == null)
            {
                return this;
            }
            _fixtureHtml += $"<b>Football Fixtures\n</b>";
            _fixtureHtml += $"<b>{fixture.HomeTeam} vs {fixture.AwayTeam}\n</b>";
            _fixtureHtml += $"{fixture.Kickoff.ToShortTimeString()}\n";
            return this;
        }

        public IHtmlContentBuilder WithRssFeedContent(Dictionary<string, List<SyndicationItem>> syndicationDict)
        {
            foreach (var kVP in syndicationDict)
            {
                _feedHtml += $"<b>{kVP.Key}</b>\n";
                _feedHtml += ParseHtmlFromSyndicationItems(kVP.Value);
            }
            return this;
        }

        private static string ParseHtmlFromSyndicationItems(List<SyndicationItem> syndicationList)
        {
            string returnBody = string.Empty;
            foreach (SyndicationItem item in syndicationList)
            {
                returnBody += $"<a href=\"{item.Links[0].Uri}\" target=\"blank\">{item.Title.Text}</a>\n";
            }
            return returnBody + "\n"; ;
        }
    }
}
