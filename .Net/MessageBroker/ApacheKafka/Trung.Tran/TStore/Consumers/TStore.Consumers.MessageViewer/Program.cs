using Confluent.Kafka;
using System;

namespace TStore.Consumers.MessageViewer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool stop = false;

            ConsumerConfig config = new ConsumerConfig
            {
                GroupId = "MessageViewer",
                BootstrapServers = "localhost:9093,localhost:9095,localhost:9097",
                SslKeyPassword = "123456",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "consumer",
                SaslPassword = "123456",
                EnableAutoCommit = false
            };

            using (IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                try
                {
                    while (!stop)
                    {
                        try
                        {
                            Console.Clear();
                            Console.Write("Choose a topic: ");
                            string topic = Console.ReadLine();
                            Console.Write("Choose a partition: ");
                            int partition = int.Parse(Console.ReadLine());
                            WatermarkOffsets offsets = consumer.QueryWatermarkOffsets(new TopicPartition(topic, partition), TimeSpan.FromSeconds(10));
                            Console.WriteLine($"Current available offset: {offsets.Low} - {offsets.High}");
                            Console.Write("Choose an offset to begin consuming: ");
                            int offset = int.Parse(Console.ReadLine());
                            ReadAllMessages(consumer, topic, partition, offset);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex);
                            stop = true;
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        static void ReadAllMessages(IConsumer<string, string> consumer, string topic, int partition, int offset)
        {
            consumer.Assign(new TopicPartitionOffset(topic, partition, offset));

            bool empty = false;

            while (!empty)
            {
                ConsumeResult<string, string> message = consumer.Consume(1000);

                if (message != null)
                {
                    Console.WriteLine($"======= {message.Message.Timestamp} =======");
                    Console.WriteLine($"Key: {message.Message.Key}");
                    Console.WriteLine($"Value: {message.Message.Value}");
                }
                else
                {
                    empty = true;
                }
            }

            Console.WriteLine("Press any to continue.");
            Console.ReadLine();
        }
    }
}
