using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProcessImage.Models;
using System.Text;
using System.Threading.Tasks;

namespace ProcessImage.Services
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _config;
        private const string _topicName = "processedimage";
        public MessageBusService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(FileMetadata fileMetadata)
        {
            var clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            var client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus"), clientOptions);
            var sender = client.CreateSender(_topicName);

            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fileMetadata)))
            {
                Subject = fileMetadata.Name,
                ApplicationProperties =
                {
                    { "color-red", fileMetadata.Color.Red },
                    { "color-green", fileMetadata.Color.Green },
                    { "color-blue", fileMetadata.Color.Blue },
                    { "category", fileMetadata.Category },
                }
            };

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessageAsync(message);
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
