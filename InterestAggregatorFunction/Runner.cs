using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InterestAggregatorFunction
{
    public class Runner
    {
        private readonly ILogger _logger;

        public Runner(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Runner>();
        }

        [Function("Runner")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            //Register dependencies
            ServiceCollection services = new();
            services = RegisterDependencies(services);
            var serviceProvider = services.BuildServiceProvider();
            EveningEmail eveningEmail = serviceProvider.GetRequiredService<EveningEmail>();
            eveningEmail.Run();
        }

        public static ServiceCollection RegisterDependencies(ServiceCollection services, FeedStorageYaml? feedStorageYamlOverride = null)
        {
            services.AddTransient<EveningEmail>();
            services.AddTransient<IEmailManager, AzureEmailManager>();
            services.AddTransient<IFeedManager, FeedManager>();
            services.AddTransient<IFeedFilter, FeedFilter>();
            services.AddTransient<IHtmlContentBuilder, HtmlContentBuilder>();
            services.AddTransient<IFeedConfig, FeedConfig>();

            if (feedStorageYamlOverride != null)
            {
                services.AddSingleton<IFeedStorage>(feedStorageYamlOverride);
            }
            else
            {
                services.AddTransient<IFeedStorage, FeedStorageYaml>();
            }

            return services;
        }
    }
}
