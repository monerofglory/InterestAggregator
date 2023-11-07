using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.FeedServices;
using InterestAggregatorFunction.Services.FeedStorage;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RssFeedFunction.Services.FeedStorage
{
    public class FeedStorageYaml : IFeedStorage
    {
        private readonly IFeedConfig _config;
        private const string _relativeYamlLocation = "\\YamlFeedList.yml";

        public FeedStorageYaml(IFeedConfig config) {
            _config = config;
        }
        public Feed[] GetFeeds()
        {
            //Fetch the YAML
            string path = GetYamlPath();
            var yaml = File.ReadAllText(path);

            return DeserializeYamlToFeedListDto(yaml).FeedList;
        }

        private static FeedListDto DeserializeYamlToFeedListDto(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<FeedListDto>(yaml);
        }

        private string GetYamlPath()
        {
            return _config.YamlPath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + _relativeYamlLocation;
        }
    }
}
