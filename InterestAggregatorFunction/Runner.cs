using System;
using InterestAggregatorFunction.Config;
using InterestAggregatorFunction.Services.AzureEmailManager;
using InterestAggregatorFunction.Services.EmailManager;
using InterestAggregatorFunction.Services.FeedManager;
using InterestAggregatorFunction.Services.FeedStorage;
using InterestAggregatorFunction.Services.Filter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RssFeedFunction.Services.FeedStorage;

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
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            //Register dependencies
            ServiceCollection services = new();
            services = RegisterDependencies(services);
            var serviceProvider = services.BuildServiceProvider();
            EveningEmail eveningEmail = serviceProvider.GetService<EveningEmail>();
            eveningEmail.Run();
        }

        public static ServiceCollection RegisterDependencies(ServiceCollection services, FeedStorageYaml feedStorageYamlOverride = null)
        {
            services.AddTransient<EveningEmail>();
            services.AddTransient<IEmailManager, AzureEmailManager>();
            services.AddTransient<IFeedManager, FeedManager>();
            services.AddTransient<IFeedFilter, FeedFilter>();
            //services.AddTransient<IHtmlGenerator, HtmlGenerator>();
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
