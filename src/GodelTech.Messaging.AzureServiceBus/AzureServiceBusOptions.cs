using System.Collections.Generic;

namespace GodelTech.Messaging.AzureServiceBus
{
    /// <summary>
    /// Azure Service Bus options
    /// </summary>
    public class AzureServiceBusOptions
    {
        /// <summary>
        /// Queue dictionary that stores key value pair of internal queue key and Azure queue name.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        // You can suppress the warning if the property is part of a Data Transfer Object (DTO) class.
        public IDictionary<string, string> Queues { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
