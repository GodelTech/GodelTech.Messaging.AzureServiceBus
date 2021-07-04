using System;
using Azure.Messaging.ServiceBus;
using GodelTech.Messaging.AzureServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extensions to register AzureServiceBusSender with the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register AzureServiceBusSender with the service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="connectionString">Connection string to Azure Service Bus.</param>
        /// <param name="configureOptions">Azure Service Bus options.</param>
        /// <returns></returns>
        public static IServiceCollection AddAzureServiceBusSender(
            this IServiceCollection services,
            Func<IConfiguration, string> connectionString,
            Action<AzureServiceBusOptions, IConfiguration> configureOptions)
        {
            // ServiceBusClient
            services.AddSingleton(
                provider => new ServiceBusClient(
                    connectionString(
                        provider.GetService<IConfiguration>()
                    )
                )
            );

            // Options
            services.AddOptions<AzureServiceBusOptions>()
                .Configure(configureOptions);

            // AzureServiceBusSender
            services.AddTransient<IAzureServiceBusSender, AzureServiceBusSender>();

            return services;
        }
    }
}