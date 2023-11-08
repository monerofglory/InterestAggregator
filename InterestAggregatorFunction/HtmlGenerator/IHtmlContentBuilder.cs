using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace InterestAggregatorFunction.Services.HtmlContentBuilder
{
    public interface IHtmlContentBuilder
    {
        public IHtmlContentBuilder WithRssFeedContent(Dictionary<string, List<SyndicationItem>> syndicationDict);
        public IHtmlContentBuilder WithFixtureContent(string title, string time);
        public string Build();

    }
}
