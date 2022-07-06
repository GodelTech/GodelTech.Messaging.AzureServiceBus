using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using GodelTech.Messaging.AzureServiceBus.Tests.Fakes;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GodelTech.Messaging.AzureServiceBus.Tests
{
    public class AzureServiceBusSenderTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;

        public AzureServiceBusSenderTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AzureServiceBusSender(_mockServiceBusClient.Object, null)
            );

            Assert.Equal("azureServiceBusOptions", exception.ParamName);
        }

        [Fact]
        public async Task SendAsync_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var model = new List<FakeModel>();

            var azureServiceBusOptions = new AzureServiceBusOptions
            {
                Queues = new Dictionary<string, string>
                {
                    {
                        "OtherInternalKey",
                        "OtherAzureServiceBusQueueName"
                    }
                }
            };

            var mockAzureServiceBusOptions = new Mock<IOptions<AzureServiceBusOptions>>();
            mockAzureServiceBusOptions
                .Setup(x => x.Value)
                .Returns(azureServiceBusOptions);

            var sender = new AzureServiceBusSender(_mockServiceBusClient.Object, mockAzureServiceBusOptions.Object);

            var expectedException = new ArgumentOutOfRangeException($"queueKey", "InternalKey", "No queue found with provided key.");

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    () =>
                        sender.SendAsync("InternalKey", model)
                );

            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal("queueKey", exception.ParamName);
            Assert.Equal("InternalKey", exception.ActualValue);
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public async Task SendAsync_Success()
        {
            // Arrange
            var model = new List<FakeModel>
            {
                new FakeModel
                {
                    Id = 1,
                    Name = "TestName"
                }
            };

            var serializedModel = JsonSerializer.Serialize(model);

            var azureServiceBusOptions = new AzureServiceBusOptions
            {
                Queues = new Dictionary<string, string>
                {
                    {
                        "InternalKey",
                        "AzureServiceBusQueueName"
                    }
                }
            };

            var mockAzureServiceBusOptions = new Mock<IOptions<AzureServiceBusOptions>>();
            mockAzureServiceBusOptions
                .Setup(x => x.Value)
                .Returns(azureServiceBusOptions);

            var sender = new AzureServiceBusSender(_mockServiceBusClient.Object, mockAzureServiceBusOptions.Object);

            var mockServiceBusSender = new Mock<ServiceBusSender>(MockBehavior.Strict);
            mockServiceBusSender
                .Setup(
                    x => x.SendMessageAsync(
                        It.Is<ServiceBusMessage>(
                            serviceBusMessage =>
                                serviceBusMessage.Body.ToString() == serializedModel
                        ),
                        It.Is<CancellationToken>(
                            cancellationToken => cancellationToken == default
                        )
                    )
                )
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mockServiceBusClient
                .Setup(
                    x => x.CreateSender(
                        It.Is<string>(queueOrTopicName => queueOrTopicName == "AzureServiceBusQueueName")
                    )
                )
                .Returns(mockServiceBusSender.Object)
                .Verifiable();

            // Act
            await sender.SendAsync("InternalKey", model);

            // Assert
            _mockServiceBusClient.VerifyAll();
            mockServiceBusSender.VerifyAll();
        }
    }
}
