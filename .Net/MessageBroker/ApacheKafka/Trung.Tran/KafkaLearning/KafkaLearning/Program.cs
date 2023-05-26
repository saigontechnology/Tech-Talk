using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KafkaLearning
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AdminClientConfig adminConfig = new AdminClientConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "AdminClient",
                SecurityProtocol = SecurityProtocol.Plaintext
            };
            using IAdminClient adminClient = new AdminClientBuilder(adminConfig).Build();

            string choice = null;

            while (choice?.Trim() != "4")
            {
                Console.Clear();
                Console.WriteLine("==== Welcome to Kafka Admin ====");
                Console.WriteLine("1. Create a topic");
                Console.WriteLine("2. Delete a topic");
                Console.WriteLine("3. Print broker metadata");
                Console.WriteLine("4. Exit");
                Console.Write("Select a choice: ");
                choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": await CreateTopicAsync(adminClient); break;
                        case "2": await DeleteTopicAsync(adminClient); break;
                        case "3": PrintBrokerMetadata(adminClient); break;
                        case "4": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

                Console.WriteLine("======================");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }
        }

        static async Task CreateTopicAsync(IAdminClient adminClient)
        {
            Console.Write("Enter topic name: ");
            string topicName = Console.ReadLine();
            Console.Write("Enter number of partitions: ");
            int numPartitions = int.Parse(Console.ReadLine());

            await adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification
                {
                    Name = topicName,
                    NumPartitions = numPartitions
                }
            });
        }

        static async Task DeleteTopicAsync(IAdminClient adminClient)
        {
            Console.Write("Enter topic name: ");
            string topicName = Console.ReadLine();
            await adminClient.DeleteTopicsAsync(new[] { topicName });
        }

        static void PrintBrokerMetadata(IAdminClient adminClient)
        {
            Metadata metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            Console.WriteLine(JsonConvert.SerializeObject(metadata, Formatting.Indented));
        }
    }
}
