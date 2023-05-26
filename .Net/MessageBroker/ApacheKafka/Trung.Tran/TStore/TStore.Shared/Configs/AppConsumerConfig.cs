using Confluent.Kafka;

namespace TStore.Shared.Configs
{
    public class AppConsumerConfig : ConsumerConfig
    {
        public AppConsumerConfig()
        {
        }

        public AppConsumerConfig(ClientConfig config) : base(config)
        {
        }

        public int ConsumerCount { get; set; }
        public int ProcessingBatchSize { get; set; }

        public AppConsumerConfig Clone()
        {
            return new AppConsumerConfig(this)
            {
                ConsumerCount = this.ConsumerCount,
                ProcessingBatchSize = this.ProcessingBatchSize,
            };
        }
    }
}
