using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace KafkaLearning.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==== Welcome to Kafka Consumer ====");

            string bootstrapServers = Environment.GetEnvironmentVariable("BootstrapServers");
            string topic = Environment.GetEnvironmentVariable("Topic");
            bool stop = false;

            if (string.IsNullOrEmpty(topic))
            {
                Console.Write("Enter topic name: ");
                topic = Console.ReadLine();
            }

            Console.WriteLine($"Consumer Id {new Random().Next(1000)} is running!");

            while (!stop)
            {
                try
                {
                    ConsumerConfig consumerConfig = new ConsumerConfig
                    {
                        GroupId = "ConsumerClient",
                        BootstrapServers = string.IsNullOrWhiteSpace(bootstrapServers) ? "localhost:9092" : bootstrapServers,
                        ClientId = "ConsumerClient",
                        SecurityProtocol = SecurityProtocol.Plaintext,
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnableAutoCommit = true,
                    };

                    using (IConsumer<string, string> consumer
                        = new ConsumerBuilder<string, string>(consumerConfig).Build())
                    {
                        try
                        {
                            consumer.Subscribe(topic);

                            while (!stop)
                            {
                                ConsumeResult<string, string> message = consumer.Consume();
                                Console.WriteLine($"==== {message.Message.Timestamp} ====");
                                Console.WriteLine(JsonConvert.SerializeObject(message, Formatting.Indented));
                            }
                        }
                        finally
                        {
                            consumer.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    Console.WriteLine("Sleep 5s ...");
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
