using System;
using System.Threading.Tasks;

namespace GodelTech.Messaging.AzureServiceBus
{
    /// <summary>
    /// Interface of Azure Service Bus sender
    /// </summary>
    public interface IAzureServiceBusSender
    {
        /// <summary>
        /// Asynchronously sends TModel object as JSON to Azure Service Bus queue.
        /// Queue is select by key from options.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queueKey">Queue key.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentOutOfRangeException">No queue found with provided key.</exception>
        public Task SendAsync<TModel>(string queueKey, TModel model)
            where TModel : class;
    }
}