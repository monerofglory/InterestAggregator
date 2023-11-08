using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services.HtmlGenerator
{
    public interface IHtmlGenerator
    {
        IHtmlGenerator ConstructFeedHtml(Dictionary<string, List<SyndicationItem>> syndicationDict);
        IHtmlGenerator ConstructOtherHtml(string title, string content);
        IHtmlGenerator Begin();
        string End();
    }
}
