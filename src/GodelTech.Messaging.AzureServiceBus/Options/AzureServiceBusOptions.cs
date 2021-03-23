using System.Collections.Generic;

namespace GodelTech.Messaging.AzureServiceBus.Options
{
    /// <summary>
    /// Azure Service Bus options
    /// </summary>
    public class AzureServiceBusOptions
    {
        /// <summary>
        /// Queue dictionary that stores key value pair of internal queue key and Azure queue name.
        /// </summary>
        public IDictionary<string, string> Queues { get; set; }
    }
}