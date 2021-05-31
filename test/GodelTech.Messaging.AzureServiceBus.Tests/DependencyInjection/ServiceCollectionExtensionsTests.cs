using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
            Expression<Func<IConfiguration, string>> connectionStringExpression = configuration => configuration
                .GetValue<string>("AzureServiceBusOptions:ConnectionString");

            Action<AzureServiceBusOptions, IConfiguration> configureOptions = (options, configuration) => configuration
                .GetSection("AzureServiceBusOptions")
                .Bind(options);

            var services = new ServiceCollection();

            services
                .AddTransient<IConfiguration>(
                    _ => new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                ["AzureServiceBusOptions:ConnectionString"] = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YourAccessKey",
                                ["AzureServiceBusOptions:Queues:FirstInternalKey"] = "FirstAzureServiceBusQueueName",
                                ["AzureServiceBusOptions:Queues:SecondInternalKey"] = "SecondAzureServiceBusQueueName"
                            }
                        )
                        .Build()
                );

            // Act
            services.AddAzureServiceBusSender(connectionStringExpression.Compile(), configureOptions);

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