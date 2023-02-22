using System;
using Azure.Identity;
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
        /// <param name="configureOptions">Azure Service Bus options.</param>
        /// <returns></returns>
        public static IServiceCollection AddAzureServiceBusSender(
            this IServiceCollection services,
            string connectionString,
            Action<AzureServiceBusOptions> configureOptions)
        {
            return services.AddAzureServiceBusSender(
                _ => new ServiceBusClient(
                    connectionString
                ),
                configureOptions
            );
        }

        /// <summary>
        /// Register AzureServiceBusSender with the service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="fullyQualifiedNamespace">The fully qualified Service Bus namespace to connect to.</param>
        /// <param name="credential">The Azure managed identity credential to use for authorization.</param>
        /// <param name="configureOptions">Azure Service Bus options.</param>
        /// <returns></returns>
        public static IServiceCollection AddAzureServiceBusSender(
            this IServiceCollection services,
            string fullyQualifiedNamespace,
            ManagedIdentityCredential credential,
            Action<AzureServiceBusOptions> configureOptions)
        {
            return services.AddAzureServiceBusSender(
                _ => new ServiceBusClient(
                    fullyQualifiedNamespace,
                    credential
                ),
                configureOptions
            );
        }

        private static IServiceCollection AddAzureServiceBusSender(
            this IServiceCollection services,
            Func<IServiceProvider, ServiceBusClient> serviceBusClient,
            Action<AzureServiceBusOptions> configureOptions)
        {
            // ServiceBusClient
            services.AddSingleton(serviceBusClient);

            // Options
            services.AddOptions<AzureServiceBusOptions>()
                .Configure(configureOptions);

            // AzureServiceBusSender
            services.AddTransient<IAzureServiceBusSender, AzureServiceBusSender>();

            return services;
        }
    }
}
