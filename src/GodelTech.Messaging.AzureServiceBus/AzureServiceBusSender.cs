using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

[assembly: CLSCompliant(false)]
namespace GodelTech.Messaging.AzureServiceBus
{
    /// <summary>
    /// Azure Service Bus sender
    /// </summary>
    public class AzureServiceBusSender : IAzureServiceBusSender
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly AzureServiceBusOptions _azureServiceBusOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSender"/> class.
        /// </summary>
        /// <param name="serviceBusClient">Azure Service Bus client.</param>
        /// <param name="azureServiceBusOptions">Azure Service Bus options.</param>
        public AzureServiceBusSender(ServiceBusClient serviceBusClient, IOptions<AzureServiceBusOptions> azureServiceBusOptions)
        {
            if (azureServiceBusOptions == null) throw new ArgumentNullException(nameof(azureServiceBusOptions));

            _serviceBusClient = serviceBusClient;
            _azureServiceBusOptions = azureServiceBusOptions.Value;
        }

        /// <summary>
        /// Asynchronously sends TModel object as JSON to Azure Service Bus queue.
        /// Queue is select by key from options.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queueKey">Queue key.</param>
        /// <param name="model">The model.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <exception cref="ArgumentOutOfRangeException">No queue found with provided key.</exception>
        public Task SendAsync<TModel>(string queueKey, TModel model, CancellationToken cancellationToken = default)
            where TModel : class
        {
            if (!_azureServiceBusOptions.Queues.TryGetValue(queueKey, out var queueName)) throw new ArgumentOutOfRangeException(nameof(queueKey), queueKey, "No queue found with provided key.");

            return SendInternalAsync(queueName, model, cancellationToken);
        }

        private async Task SendInternalAsync<TModel>(string queueName, TModel model, CancellationToken cancellationToken)
            where TModel : class
        {
            var messageToSend = new ServiceBusMessage(JsonSerializer.Serialize(model));

            await using var sender = _serviceBusClient.CreateSender(queueName);

            await sender.SendMessageAsync(messageToSend, cancellationToken);
        }
    }
}
