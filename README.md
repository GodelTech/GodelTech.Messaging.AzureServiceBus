# GodelTech.Messaging.AzureServiceBus

## Description
GodelTech.Messaging.AzureServiceBus is a .NET library designed to facilitate the integration of Azure Service Bus into your applications. It provides methods to add Azure Service Bus senders, supporting both connection strings and managed identity for authentication.

```c#
services.AddAzureServiceBusSender(
  Configuration["AzureServiceBusOptions:ConnectionString"],
  options => Configuration.Bind("AzureServiceBusOptions", options)
)
```

with `appsettings.json`

```c#
{
  "AzureServiceBusOptions": {
    "ConnectionString": "Endpoint=sb://{name}.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YourAccessKey",
    "Queues": {
      "exampleQueue": "example.servicebus.queue"
    }
  }
}
```

Allows to add Azure Service Bus with Managed Identity
```c#
services.AddAzureServiceBusSender(
  Configuration["AzureServiceBusOptions:FullyQualifiedNamespace"],
  new ManagedIdentityCredential(),
  options => Configuration.Bind("AzureServiceBusOptions", options)
)
```

with `appsettings.json`

```c#
{
  "AzureServiceBusOptions": {
    "FullyQualifiedNamespace": "{name}.servicebus.windows.net",
    "Queues": {
      "exampleQueue": "example.servicebus.queue"
    }
  }
}
```

## License
This project is licensed under the MIT License. See the LICENSE file for more details.