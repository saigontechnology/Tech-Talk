using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KafkaLearning.Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ProducerConfig producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "ProducerClient",
                SecurityProtocol = SecurityProtocol.Plaintext
            };
            using IProducer<string, string> producer = new ProducerBuilder<string, string>(producerConfig).Build();

            string choice = null;

            while (choice?.Trim() != "4")
            {
                Console.Clear();
                Console.WriteLine("==== Welcome to Kafka Producer ====");
                Console.WriteLine("1. Send async message");
                Console.WriteLine("2. Send message and wait for result");
                Console.WriteLine("3. Auto send messages");
                Console.WriteLine("4. Exit");
                Console.Write("Select a choice: ");
                choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": SendAsyncMessage(producer); break;
                        case "2": await SendMessageAndWaitAsync(producer); break;
                        case "3": AutoSendMessages(producer); break;
                        case "4": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

                Console.WriteLine("======================");
                Console.WriteLine("Press enter to continue!");
                Console.ReadLine();
            }
        }

        static void SendAsyncMessage(IProducer<string, string> producer)
        {
            Console.Write("Enter topic: ");
            string topic = Console.ReadLine();
            Console.Write("Enter message key: ");
            string key = Console.ReadLine();
            Console.Write("Enter message value: ");
            string value = Console.ReadLine();
            producer.Produce(topic, new Message<string, string>
            {
                Key = key,
                Value = value
            }, report =>
            {
                Console.WriteLine("=== Produce Report ===");
                Console.WriteLine(JsonConvert.SerializeObject(report, Formatting.Indented));
            });
        }

        static async Task SendMessageAndWaitAsync(IProducer<string, string> producer)
        {
            Console.Write("Enter topic: ");
            string topic = Console.ReadLine();
            Console.Write("Enter message key: ");
            string key = Console.ReadLine();
            Console.Write("Enter message value: ");
            string value = Console.ReadLine();
            DeliveryResult<string, string> result = await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = value
            });
            Console.WriteLine("=== Produce Result ===");
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        static void AutoSendMessages(IProducer<string, string> producer)
        {
            Console.Write("Enter topic: ");
            string topic = Console.ReadLine();
            Console.Write("Enter delay: ");
            int delay = int.Parse(Console.ReadLine());
            bool stop = false;
            Task task = Task.Run(async () =>
            {
                while (!stop)
                {
                    string key = Guid.NewGuid().ToString();
                    string value = DateTime.Now.ToString();

                    DeliveryResult<string, string> result = await producer.ProduceAsync(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = value
                    });

                    Console.WriteLine($"Produced message key {key} - value {value}");

                    await Task.Delay(delay);
                }
            });

            Console.WriteLine("Press enter to stop!");
            Console.ReadLine();
            stop = true;
        }

    }
}
