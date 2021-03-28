using System;
using System.Collections.Generic;
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
        public void AddAzureServiceBusSender_Success()
        {
            // Arrange
            const string connectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YourAccessKey";
            var queues = new Dictionary<string, string>
            {
                {
                    "InternalKey",
                    "AzureServiceBusQueueName"
                }
            };
            Action<AzureServiceBusOptions> optionsAction = options =>
            {
                options.Queues = queues;
            };

            var services = new ServiceCollection();

            // Act
            services.AddAzureServiceBusSender(connectionString, optionsAction);

            // Assert
            var provider = services.BuildServiceProvider();

            var resultRequiredService = provider.GetRequiredService<ServiceBusClient>();
            Assert.NotNull(resultRequiredService);

            var resultOptionsAction = provider.GetRequiredService<IOptions<AzureServiceBusOptions>>();
            Assert.NotNull(resultOptionsAction);
            Assert.NotNull(resultOptionsAction.Value);
            Assert.Equal(queues, resultOptionsAction.Value.Queues);

            var resultAzureServiceBusSender = provider.GetRequiredService<IAzureServiceBusSender>();
            Assert.NotNull(resultAzureServiceBusSender);
            Assert.IsType<AzureServiceBusSender>(resultAzureServiceBusSender);
        }
    }
}