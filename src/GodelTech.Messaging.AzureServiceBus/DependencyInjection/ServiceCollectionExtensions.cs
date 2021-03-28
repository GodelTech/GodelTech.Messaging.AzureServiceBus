using System;
using Azure.Messaging.ServiceBus;
using GodelTech.Messaging.AzureServiceBus;
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
        /// <param name="optionsAction">Azure Service Bus options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddAzureServiceBusSender(
            this IServiceCollection services,
            string connectionString,
            Action<AzureServiceBusOptions> optionsAction)
        {
            // ServiceBusClient
            services.AddTransient(provider => new ServiceBusClient(connectionString));

            // Options
            services.Configure(optionsAction);

            // AzureServiceBusSender
            services.AddTransient<AzureServiceBusSender>();

            return services;
        }
    }
}