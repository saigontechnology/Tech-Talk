using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

namespace TStore.InteractionApi.Consumers
{
    public interface ISaveInteractionConsumer
    {
        void StartListenThread(int id);
    }

    public class SaveInteractionConsumer : ISaveInteractionConsumer
    {
        private readonly AppConsumerConfig _baseConfig;
        private readonly IServiceProvider _provider;
        private readonly IApplicationLog _log;
        private bool _cancelled;
        private int? _id;

        public SaveInteractionConsumer(IConfiguration configuration,
            IServiceProvider provider,
            IApplicationLog log)
        {
            _provider = provider;
            _log = log;
            _baseConfig = new AppConsumerConfig();
            configuration.Bind("SaveInteractionConsumerConfig", _baseConfig);

            if (configuration.GetValue<bool>("StartFromVS"))
            {
                _baseConfig.FindCertIfNotFound();
            }
        }

        public void StartListenThread(int id)
        {
            if (_id != null) throw new Exception("Already started");

            _id = id;

            Thread thread = new Thread(async () =>
            {
                while (!_cancelled)
                {
                    try
                    {
                        await ListenForNewInteractionAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                        await Task.Delay(7000);
                    }
                }
            })
            {
                IsBackground = true
            };

            thread.Start();
        }

        private async Task ListenForNewInteractionAsync()
        {
            await _log.LogAsync("[SAVE INTERACTION HANDLER]");

            using (IConsumer<string, InteractionModel> consumer
                = new ConsumerBuilder<string, InteractionModel>(_baseConfig)
                    .SetValueDeserializer(new SimpleJsonSerdes<InteractionModel>())
                    .Build())
            {
                try
                {
                    consumer.Subscribe(EventConstants.Events.NewUnsavedInteraction);

                    List<ConsumeResult<string, InteractionModel>> batch = new List<ConsumeResult<string, InteractionModel>>();
                    bool isTimeout = false;

                    while (!_cancelled)
                    {
                        ConsumeResult<string, InteractionModel> message = consumer.Consume(100);

                        if (message != null)
                        {
                            await _log.LogAsync($"Consumer {_id} is handling message {message.Message.Key} - {message.Message.Timestamp.UtcDateTime}");

                            batch.Add(message);
                        }
                        else
                        {
                            isTimeout = true;
                        }

                        if (batch.Count > 0 && (batch.Count >= _baseConfig.ProcessingBatchSize || isTimeout))
                        {
                            await _log.LogAsync($"Consumer {_id} begins processing batch of {batch.Count} messages");

                            using (IServiceScope scope = _provider.CreateScope())
                            {
                                List<InteractionModel> interactions = batch
                                    .Select(m => m.Message.Value)
                                    .ToList();

                                await HandleNewInteractionsAsync(scope.ServiceProvider, interactions);
                            }

                            try
                            {
                                consumer.Commit();
                            }
                            catch (Exception ex)
                            {
                                await _log.LogAsync(ex.Message);
                            }

                            batch.Clear();
                            isTimeout = false;
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        private async Task HandleNewInteractionsAsync(IServiceProvider serviceProvider, List<InteractionModel> interactions)
        {
            IInteractionService interactionService = serviceProvider.GetService<IInteractionService>();

            await interactionService.SaveInteractionsAsync(interactions);

            await _log.LogAsync($"Finish saving interaction batch of {interactions.Count}");
        }
    }
}
