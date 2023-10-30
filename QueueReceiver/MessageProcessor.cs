namespace QueueSender;

using Azure.Messaging.ServiceBus;

public class MessageProcessor
{
    public async Task ProcessMessage()
    {
        ServiceBusClient client;
        ServiceBusProcessor processor;

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        client = new ServiceBusClient(
            "Endpoint=sb://service-bus-ecommerce.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=3W1kxYQiSrsgLnpC/PKGH6DEBbbv4e21O+ASbKaNjJ8=",
            clientOptions);

        processor = client.CreateProcessor("my-queue", new ServiceBusProcessorOptions());

        try
        {
            processor.ProcessMessageAsync += MessageReceiver.MessageHandler;

            processor.ProcessErrorAsync += MessageReceiver.ErrorHandler;

            await processor.StartProcessingAsync();
            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }
        finally
        {
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}