using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

namespace TStore.Consumers.InteractionAggregator
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLog _log;
        private readonly AppConsumerConfig _baseConfig;
        private bool _cancelled;

        public Worker(IServiceProvider serviceProvider, IConfiguration configuration,
            IApplicationLog log)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _log = log;
            _baseConfig = new AppConsumerConfig();
            _configuration.Bind("InteractionAggregatorConsumerConfig", _baseConfig);

            if (_configuration.GetValue<bool>("StartFromVS"))
            {
                _baseConfig.FindCertIfNotFound();
            }
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
            AppConsumerConfig configClone = _baseConfig.Clone();
            configClone.GroupInstanceId = $"{idx}";

            using (IConsumer<string, IEnumerable<InteractionModel>> consumer
                    = new ConsumerBuilder<string, IEnumerable<InteractionModel>>(configClone)
                        .SetValueDeserializer(new SimpleJsonSerdes<IEnumerable<InteractionModel>>())
                        .Build())
            {
                try
                {
                    consumer.Subscribe(EventConstants.Events.NewRecordedInteraction);

                    while (!_cancelled)
                    {
                        ConsumeResult<string, IEnumerable<InteractionModel>> message = consumer.Consume(default(CancellationToken));

                        await _log.LogAsync($"Consumer {idx} begins handle message {message.Message.Timestamp.UtcDateTime}");

                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            await HandleNewInteractionsAsync(message.Message.Value, scope.ServiceProvider);
                        }

                        try
                        {
                            consumer.Commit();
                        }
                        catch (Exception ex)
                        {
                            await _log.LogAsync(ex.Message);
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        private async Task HandleNewInteractionsAsync(IEnumerable<InteractionModel> interactionModels,
            IServiceProvider serviceProvider)
        {
            IInteractionService interactionService = serviceProvider.GetRequiredService<IInteractionService>();

            await interactionService.AggregateInteractionReportAsync(interactionModels);

            await _log.LogAsync($"Finish aggregating interaction reports for {interactionModels.Count()} interactions");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _log.LogAsync("[INTERACTION AGGREGATOR]");

            for (int i = 0; i < _baseConfig.ConsumerCount; i++)
            {
                StartConsumerThread(i);
            }
        }
    }
}
