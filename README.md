# GodelTech.Messaging.AzureServiceBus

Allows to add Azure Service Bus

```c#
services.AddAzureServiceBusSender(
    configuration => configuration
        .GetValue<string>("AzureServiceBusOptions:ConnectionString"),
    (options, configuration) => configuration
        .GetSection("AzureServiceBusOptions")
        .Bind(options)
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