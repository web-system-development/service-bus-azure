using Azure.Messaging.ServiceBus;

// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the sender used to publish messages to the topic
ServiceBusSender sender;

// number of messages to be sent to the topic

// The Service Bus client types are safe to cache and use as a singleton for the lifetime
// of the application, which is best practice when messages are being published or read
// regularly.
client = new ServiceBusClient(
    "Endpoint=sb://service-bus-odin.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=d5s0tOOJd1cfHwXywUixeg0ooEOmffhCT+ASbJ6sLNI=");
sender = client.CreateSender("topic-odin");

while (true)
{
    var value = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(value)) break;
    ServiceBusMessage message = new ServiceBusMessage(value)
    {
        // TODO: Edit here to send messages to different subscriptions
        Subject = "audit",
    };

    await sender.SendMessageAsync(message);
}

// Calling DisposeAsync on client types is required to ensure that network
// resources and other unmanaged objects are properly cleaned up.
await sender.DisposeAsync();
await client.DisposeAsync();

Console.WriteLine("Press any key to end the application");
Console.ReadKey();