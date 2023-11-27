using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InterestAggregatorFunction
{
    public class Runner(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<Runner>();

        [Function("Runner")]
        public void Run([TimerTrigger("0 0 21 * * *")] TimerInfo myTimer)
        {
            //Register dependencies
            ServiceCollection services = new();
            services = RegisterDependencies(services);
            var serviceProvider = services.BuildServiceProvider();
            EveningEmail eveningEmail = serviceProvider.GetRequiredService<EveningEmail>();
            eveningEmail.Run();
        }

        [Function("Warmer")]
        public void Warm([TimerTrigger("0 50 20 * * *")] TimerInfo myTimer)
        {
            //Warm up the FixtureFetcher service
            new HttpClient().Send(new HttpRequestMessage(HttpMethod.Get, $"https://fixturefetcherservice.azurewebsites.net/fixturefetcher/"));
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
