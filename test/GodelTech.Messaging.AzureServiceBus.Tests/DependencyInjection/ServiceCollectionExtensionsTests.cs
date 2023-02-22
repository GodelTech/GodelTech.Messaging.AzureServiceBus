using System.Collections.Generic;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace GodelTech.Messaging.AzureServiceBus.Tests.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAzureServiceBusSender_WithConnectionString_Success()
        {
            // Arrange
            const string connectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YourAccessKey";

            static void ConfigureOptions(AzureServiceBusOptions options)
            {
                options.Queues = new Dictionary<string, string>
                {
                    ["FirstInternalKey"] = "FirstAzureServiceBusQueueName",
                    ["SecondInternalKey"] = "SecondAzureServiceBusQueueName"
                };
            }

            var services = new ServiceCollection();

            // Act
            services.AddAzureServiceBusSender(connectionString, ConfigureOptions);

            // Assert
            var provider = services.BuildServiceProvider();

            var resultRequiredService = provider.GetRequiredService<ServiceBusClient>();
            Assert.NotNull(resultRequiredService);

            var resultOptionsAction = provider.GetRequiredService<IOptions<AzureServiceBusOptions>>();
            Assert.NotNull(resultOptionsAction);
            Assert.NotNull(resultOptionsAction.Value);
            Assert.Equal("FirstAzureServiceBusQueueName", resultOptionsAction.Value.Queues["FirstInternalKey"]);
            Assert.Equal("SecondAzureServiceBusQueueName", resultOptionsAction.Value.Queues["SecondInternalKey"]);

            var resultAzureServiceBusSender = provider.GetRequiredService<IAzureServiceBusSender>();
            Assert.NotNull(resultAzureServiceBusSender);
            Assert.IsType<AzureServiceBusSender>(resultAzureServiceBusSender);
        }

        [Fact]
        public void AddAzureServiceBusSender_WithFullyQualifiedNamespace_Success()
        {
            // Arrange
            const string fullyQualifiedNamespace = "test.servicebus.windows.net";

            static void ConfigureOptions(AzureServiceBusOptions options)
            {
                options.Queues = new Dictionary<string, string>
                {
                    ["FirstInternalKey"] = "FirstAzureServiceBusQueueName",
                    ["SecondInternalKey"] = "SecondAzureServiceBusQueueName"
                };
            }

            var services = new ServiceCollection();

            // Act
            services.AddAzureServiceBusSender(fullyQualifiedNamespace, new ManagedIdentityCredential(), ConfigureOptions);

            // Assert
            var provider = services.BuildServiceProvider();

            var resultRequiredService = provider.GetRequiredService<ServiceBusClient>();
            Assert.NotNull(resultRequiredService);

            var resultOptionsAction = provider.GetRequiredService<IOptions<AzureServiceBusOptions>>();
            Assert.NotNull(resultOptionsAction);
            Assert.NotNull(resultOptionsAction.Value);
            Assert.Equal("FirstAzureServiceBusQueueName", resultOptionsAction.Value.Queues["FirstInternalKey"]);
            Assert.Equal("SecondAzureServiceBusQueueName", resultOptionsAction.Value.Queues["SecondInternalKey"]);

            var resultAzureServiceBusSender = provider.GetRequiredService<IAzureServiceBusSender>();
            Assert.NotNull(resultAzureServiceBusSender);
            Assert.IsType<AzureServiceBusSender>(resultAzureServiceBusSender);
        }
    }
}
