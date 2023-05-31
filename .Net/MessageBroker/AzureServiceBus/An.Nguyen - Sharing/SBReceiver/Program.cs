using Microsoft.Azure.ServiceBus;
using SBShared.Models;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SBReceiver
{
    class Program
    {
        const string connectionString = "Endpoint=sb://xuanan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IhG/ARHE9f6palXyqsM9UusOCrq+w0bq6iuF1dgP+Hc=";
        const string queueName = "simplequeue";
        static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            queueClient = new QueueClient(connectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.ReadLine();

            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var jsonString = Encoding.UTF8.GetString(message.Body);
            PersonModel person = JsonSerializer.Deserialize<PersonModel>(jsonString);
            Console.WriteLine($"Message received: First name: {person.FirstName} - Last name: {person.LastName}");
            if (Regex.IsMatch(person.FirstName, @"^[A-Za-z\s]*$") && Regex.IsMatch(person.LastName, @"^[A-Za-z\s]*$"))
            {
                Console.WriteLine($"Message process success");
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            else
            {
                Console.WriteLine($"Message process fail");
                await queueClient.AbandonAsync(message.SystemProperties.LockToken);
            }
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}
