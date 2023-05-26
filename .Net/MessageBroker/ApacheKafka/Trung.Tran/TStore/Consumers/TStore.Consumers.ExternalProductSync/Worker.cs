using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TStore.Shared.Configs;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;
using TStore.Shared.Models;
using TStore.Shared.Serdes;
using TStore.Shared.Services;

namespace TStore.Consumers.ExternalProductSync
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLog _log;
        private readonly AppConsumerConfig _baseConsumerConfig;
        private readonly AppProducerConfig _baseProducerConfig;
        private readonly int _transactionCommitInterval;
        private bool _cancelled;

        public Worker(IServiceProvider serviceProvider, IConfiguration configuration,
            IApplicationLog log)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _log = log;
            _baseConsumerConfig = new AppConsumerConfig();
            _configuration.Bind("ExternalProductSyncConsumerConfig", _baseConsumerConfig);
            _baseProducerConfig = new AppProducerConfig();
            _configuration.Bind("ExternalProductProducerConfig", _baseProducerConfig);

            if (_configuration.GetValue<bool>("StartFromVS"))
            {
                _baseConsumerConfig.FindCertIfNotFound();
                _baseProducerConfig.FindCertIfNotFound();
            }

            _transactionCommitInterval = _configuration.GetValue<int>("TransationCommitInterval");
        }

        private void StartConsumerThread(int idx)
        {
            Thread thread = new Thread(async () =>
            {
                while (!_cancelled)
                {
                    try
                    {
                        await ConsumeAsync(idx);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                        await Task.Delay(7000);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private async Task ConsumeAsync(int idx)
        {
            IKafkaProducerManager kafkaProducerManager = _serviceProvider.GetService<IKafkaProducerManager>();
            SemaphoreSlim consumeSemaphore = new SemaphoreSlim(1, 1);
            SemaphoreSlim commitSemaphore = new SemaphoreSlim(1, 1);
            Dictionary<TopicPartition, Offset> offsetToCommitMap = new Dictionary<TopicPartition, Offset>();
            CancellationTokenSource cancellationTokenSrc = new CancellationTokenSource();

            // [Important] 1 TransactionalId per producer instance
            TransactionalProducerWrapper<string, string> producerWrapper = null;

            try
            {
                producerWrapper = await kafkaProducerManager
                    .GetTransactionalProducerFromPoolAsync<string, string>(
                        _baseProducerConfig, _baseProducerConfig.DefaultPoolSize,
                        nameof(ExternalProductSync), idx, forceRecreate: true);

                using (IConsumer<string, ProductModel> consumer
                    = new ConsumerBuilder<string, ProductModel>(_baseConsumerConfig)
                        .SetValueDeserializer(new SimpleJsonSerdes<ProductModel>())
                        .SetPartitionsRevokedHandler((consumer, offsets) =>
                        {
                            commitSemaphore.Wait();
                            commitSemaphore.TryRelease();
                        })
                        .Build())
                {
                    try
                    {
                        bool inTransaction = false;
                        System.Timers.Timer commitTimer = null;

                        Func<Task> abortTransFunc = async () =>
                        {
                            try
                            {
                                inTransaction = false;
                                consumeSemaphore.TryRelease();
                                commitSemaphore.TryRelease();
                                offsetToCommitMap.Clear();
                                cancellationTokenSrc.Cancel();

                                producerWrapper.AbortTransaction();

                                await _log.LogAsync($"Aborted transaction {producerWrapper.TransactionalId}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine(ex);
                            }
                            finally
                            {
                                commitTimer?.Stop();
                                commitTimer?.Dispose();
                            }
                        };

                        consumer.Subscribe(new[]
                        {
                            EventConstants.Events.ProductCreated,
                            EventConstants.Events.ProductUpdated
                        });

                        while (!cancellationTokenSrc.IsCancellationRequested)
                        {
                            ConsumeResult<string, ProductModel> message = consumer.Consume(cancellationTokenSrc.Token);

                            await consumeSemaphore.WaitAsync();

                            if (!inTransaction)
                            {
                                await commitSemaphore.WaitAsync();

                                producerWrapper.BeginTransaction();

                                inTransaction = true;

                                await _log.LogAsync($"Entered transaction {producerWrapper.TransactionalId}");

                                commitTimer = new System.Timers.Timer()
                                {
                                    AutoReset = false,
                                    Interval = _transactionCommitInterval
                                };
                                commitTimer.Elapsed += async (obj, e) =>
                                {
                                    try
                                    {
                                        await consumeSemaphore.WaitAsync();

                                        if (offsetToCommitMap.Count > 0)
                                        {
                                            TopicPartitionOffset[] offsets = offsetToCommitMap.Select(o => new TopicPartitionOffset(o.Key, o.Value)).ToArray();

                                            producerWrapper.SendOffsetsToTransaction(
                                                offsets, consumer.ConsumerGroupMetadata,
                                                TimeSpan.FromSeconds(30));

                                            producerWrapper.CommitTransaction();

                                            offsetToCommitMap.Clear();
                                        }

                                        inTransaction = false;
                                        consumeSemaphore.TryRelease();
                                        commitSemaphore.TryRelease();

                                        await _log.LogAsync($"Committed transaction {producerWrapper.TransactionalId}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine(ex);
                                        await abortTransFunc();
                                    }
                                };
                                commitTimer.Start();
                            }

                            // [DEMO] heavy task
                            int? syncDelay = _configuration.GetValue<int?>("SyncDelay");
                            if (syncDelay > 0)
                            {
                                await Task.Delay(syncDelay.Value);
                            }

                            try
                            {
                                await _log.LogAsync($"[{message.Topic}] Consumer {idx} begins to transform product {JsonConvert.SerializeObject(message.Message.Value)}");

                                for (int i = 0; i < 10; i++)
                                {
                                    await producerWrapper.ProduceAsync(EventConstants.Events.SampleEvents,
                                        new Message<string, string>
                                        {
                                            Key = message.Message.Key,
                                            Value = $"Sample value {i}"
                                        });
                                }

                                offsetToCommitMap[message.TopicPartition] = message.Offset + 1;

                                consumeSemaphore.TryRelease();

                                await _log.LogAsync($"[{message.Topic}] Consumer {idx} finishes processing product {message.Message.Value.Id}");
                            }
                            catch (Exception ex)
                            {
                                await abortTransFunc();
                                throw ex;
                            }
                        }
                    }
                    finally
                    {
                        consumeSemaphore.TryRelease();
                        commitSemaphore.TryRelease();

                        consumer.Close();
                    }
                }
            }
            finally
            {
                if (producerWrapper != null)
                {
                    kafkaProducerManager.Release(producerWrapper);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _log.LogAsync("[EXTERNAL PRODUCT SYNC]");

            for (int i = 0; i < _baseConsumerConfig.ConsumerCount; i++)
            {
                StartConsumerThread(i);
            }
        }
    }
}
