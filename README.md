# GodelTech.Messaging.AzureServiceBus

Allows to add Azure Service Bus

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