using FixtureFetchers;
using System.ServiceModel.Syndication;

namespace InterestAggregatorFunction.Services;

public interface IHtmlContentBuilder
{
    public IHtmlContentBuilder WithRssFeedContent(Dictionary<string, List<SyndicationItem>> syndicationDict);
    public IHtmlContentBuilder WithFixtureContent(Fixture fixture);
    public string Build();

}
