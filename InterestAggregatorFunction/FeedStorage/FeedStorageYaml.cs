using InterestAggregatorFunction.Config;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace InterestAggregatorFunction.Services
{
    public class FeedStorageYaml(IFeedConfig config) : IFeedStorage
    {
        private readonly IFeedConfig _config = config;

        public Feed[] GetFeeds()
        {
            var yaml = File.ReadAllText(GetYamlPath());

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
            return _config.YamlPath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/YamlFeedList.yml";
        }
    }
}
