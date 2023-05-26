using Confluent.Kafka;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TStore.Shared.Configs
{
    public class AppProducerConfig : ProducerConfig
    {
        public AppProducerConfig()
        {
        }

        public AppProducerConfig(ClientConfig config) : base(config)
        {
        }

        public AppProducerConfig(IDictionary<string, string> config) : base(config)
        {
        }

        public bool ProduceDuplication { get; set; }
        public int DefaultPoolSize { get; set; }

        public AppProducerConfig Clone()
        {
            string configJson = JsonConvert.SerializeObject(this);
            IEnumerable<KeyValuePair<string, string>> configDictionary = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(configJson);
            AppProducerConfig clonedConfig = new AppProducerConfig(new Dictionary<string, string>(configDictionary));

            clonedConfig.ProduceDuplication = ProduceDuplication;
            clonedConfig.DefaultPoolSize = DefaultPoolSize;

            return clonedConfig;
        }
    }
}
