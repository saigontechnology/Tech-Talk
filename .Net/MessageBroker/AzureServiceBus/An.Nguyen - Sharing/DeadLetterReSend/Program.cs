using Azure.Messaging.ServiceBus;
using SBShared.Models;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DeadLetterReSend
{
    internal class Program
    {
        private static ServiceBusClient _client;
        const string connectionString = "Endpoint=sb://xuanan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IhG/ARHE9f6palXyqsM9UusOCrq+w0bq6iuF1dgP+Hc=";
        const string queueName = "simplequeue";
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            _client = new ServiceBusClient(connectionString);
            var sender = _client.CreateSender(queueName);
            var fixupTask = PickUpAndFixDeadletters(queueName, sender, cts.Token);
            await Task.WhenAny(
                Task.Run(() => Console.ReadKey())
            );

            // end the processing
            cts.Cancel();
            // await shutdown and exit
            await Task.WhenAll(fixupTask);
        }

        private static Task PickUpAndFixDeadletters(string queueName, ServiceBusSender resubmitSender, CancellationToken cancellationToken)
        {
            var doneReceiving = new TaskCompletionSource<bool>();

            // here, we create a receiver on the Deadletter queue
            ServiceBusProcessor dlqProcessor =
                _client.CreateProcessor(queueName, new ServiceBusProcessorOptions { SubQueue = SubQueue.DeadLetter, ReceiveMode = ServiceBusReceiveMode.PeekLock });

            // close the receiver and factory when the CancellationToken fires
            cancellationToken.Register(
                async () =>
                {
                    await dlqProcessor.CloseAsync();
                    doneReceiving.SetResult(true);
                });

            // register the RegisterMessageHandler callback
            dlqProcessor.ProcessMessageAsync += async args =>
            {
                // first, we create a new sendable message of the picked up message
                // that we can resubmit.
                Regex rgx = new Regex("[^a-zA-Z]");
                var jsonString = Encoding.UTF8.GetString(args.Message.Body);
                PersonModel person = JsonSerializer.Deserialize<PersonModel>(jsonString);
                Console.WriteLine($"Faulty message received: First name: {person.FirstName} - Last name: {person.LastName}");
                if (!Regex.IsMatch(person.FirstName, @"^[A-Za-z\s]*$"))
                {
                    person.FirstName = rgx.Replace(person.FirstName, "");
                }
                if (!Regex.IsMatch(person.LastName, @"^[A-Za-z\s]*$"))
                {
                    person.LastName = rgx.Replace(person.LastName, "");
                }
                Console.WriteLine($"Fixed message to re-send: First name: {person.FirstName} - Last name: {person.LastName}");
                var fixedMessage = JsonSerializer.Serialize(person);

                var resubmitMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(fixedMessage));
                Console.WriteLine(fixedMessage);
                await resubmitSender.SendMessageAsync(resubmitMessage);

                // finally complete the original message and remove it from the DLQ
                await args.CompleteMessageAsync(args.Message);
            };
            dlqProcessor.ProcessErrorAsync += LogMessageHandlerException;
            _ = dlqProcessor.StartProcessingAsync();
            return doneReceiving.Task;
        }

        private static Task LogMessageHandlerException(ProcessErrorEventArgs e)
        {
            Console.WriteLine($"Exception: \"{e.Exception.Message}\" {e.EntityPath}");
            return Task.CompletedTask;
        }
    }
}