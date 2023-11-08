using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services.HtmlGenerator
{
    public class HtmlGenerator
    {
        private const string _noNewsString = "No news today.";

        private string _returnHtml = string.Empty;
        public HtmlGenerator Begin()
        {
            _returnHtml += "<PRE>";
            return this;
        }

        public string End()
        {
            if (_returnHtml == "<PRE>")
            {
                _returnHtml = _noNewsString;
                return _returnHtml;
            }
            _returnHtml += "</PRE>";
            return _returnHtml;
        }

        public HtmlGenerator ConstructOtherHtml(string title, string content)
        {
            if (!string.IsNullOrEmpty(title))
            {
                _returnHtml += $"<b>{title}\n</b>";
                _returnHtml += $"{content}\n";
            }
            return this;
        }

        public HtmlGenerator ConstructFeedHtml(Dictionary<string, List<SyndicationItem>> syndicationDict)
        {
            foreach(var kVP in syndicationDict)
            {
                _returnHtml += $"<b>{kVP.Key}</b>\n";
                _returnHtml += ParseHtmlFromSyndicationItems(kVP.Value);
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
