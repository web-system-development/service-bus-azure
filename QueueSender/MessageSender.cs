namespace QueueSender;

using Azure.Messaging.ServiceBus;

public class MessageSender
{
    public async Task SendMessage()
    {
        ServiceBusClient client;
        ServiceBusSender sender;

        const int numberOfMessages = 3;

        // The Service Bus client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when messages are being published or read
        // regularly.
        //
        // set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
        // If you use the default AmqpTcp, you will need to make sure that the ports 5671 and 5672 are open

        // TODO: Replace the <NAMESPACE-CONNECTION-STRING> and <QUEUE-NAME> placeholders
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        client = new ServiceBusClient(
            "Endpoint=sb://service-bus-ecommerce.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=3W1kxYQiSrsgLnpC/PKGH6DEBbbv4e21O+ASbKaNjJ8=",
            clientOptions);
        sender = client.CreateSender("my-queue");

        while (true)
        {
            var value = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(value)) break;
            ServiceBusMessage message = new ServiceBusMessage(value);

            await sender.SendMessageAsync(message);
        }


        // Use the producer client to send the batch of messages to the Service Bus queue
        await sender.DisposeAsync();
        await client.DisposeAsync();
    }
}