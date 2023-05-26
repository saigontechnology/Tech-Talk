using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TStore.Shared.Configs;
using TStore.Shared.Helpers;

namespace TStore.Shared.Services
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TKey, TValue>(string eventName, TKey key, TValue value, Action<object> deliveryHandler = null);
        Task<object> PublishAndWaitAsync<TKey, TValue>(string eventName, TKey key, TValue value);
    }

    public class KafkaMessagePublisher : IMessagePublisher
    {
        private readonly AppProducerConfig _baseConfig;
        private readonly IKafkaProducerManager _kafkaProducerManager;

        public KafkaMessagePublisher(IConfiguration configuration,
            IKafkaProducerManager kafkaProducerManager)
        {
            _kafkaProducerManager = kafkaProducerManager;
            _baseConfig = new AppProducerConfig();
            configuration.Bind("CommonProducerConfig", _baseConfig);

            if (configuration.GetValue<bool>("StartFromVS"))
            {
                _baseConfig.FindCertIfNotFound();
            }
        }

        public Task PublishAsync<TKey, TValue>(string eventName, TKey key, TValue value, Action<object> deliveryHandler = null)
        {
            IProducer<TKey, TValue> producer = _kafkaProducerManager.GetCommonProducer<TKey, TValue>(eventName, _baseConfig);

            // [DEMO] async call, provide delivery result callback
            Action produceAct = () => producer.Produce(eventName, new Message<TKey, TValue>()
            {
                Key = key,
                Value = value,
                Timestamp = new Timestamp(DateTimeOffset.UtcNow),
            }, deliveryHandler);

            produceAct();

            // [DEMO] for idempotence demo
            if (_baseConfig.ProduceDuplication) produceAct();

            return Task.CompletedTask;
        }

        public async Task<object> PublishAndWaitAsync<TKey, TValue>(string eventName, TKey key, TValue value)
        {
            IProducer<TKey, TValue> producer = _kafkaProducerManager.GetCommonProducer<TKey, TValue>(eventName, _baseConfig);

            Func<Task<DeliveryResult<TKey, TValue>>> produceAct = async () => await producer.ProduceAsync(eventName, new Message<TKey, TValue>()
            {
                Key = key,
                Value = value,
                Timestamp = new Timestamp(DateTimeOffset.UtcNow),
            });

            DeliveryResult<TKey, TValue> result = await produceAct();

            // [DEMO] for idempotence demo
            if (_baseConfig.ProduceDuplication) await produceAct();

            return result;
        }
    }
}
