using Confluent.Kafka;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TStore.Shared.Configs;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;
using TStore.Shared.Models;
using TStore.Shared.Serdes;
using TStore.Shared.Services;

namespace TStore.Consumers.PromotionCalculator
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLog _log;
        private readonly AppConsumerConfig _baseConfig;
        private readonly MemoryCache _memoryCache;
        private readonly IOptions<MemoryCacheOptions> _memoryCacheOptions;
        private bool _cancelled;

        public Worker(IServiceProvider serviceProvider, IConfiguration configuration,
            IApplicationLog log,
            IOptions<MemoryCacheOptions> memCacheOptions)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _log = log;
            _baseConfig = new AppConsumerConfig();
            _memoryCacheOptions = memCacheOptions;
            _memoryCache = new MemoryCache(memCacheOptions);
            _configuration.Bind("PromotionCalculatorConsumerConfig", _baseConfig);

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
            using (IConsumer<string, OrderModel> consumer
                = new ConsumerBuilder<string, OrderModel>(_baseConfig)
                    .SetValueDeserializer(new SimpleJsonSerdes<OrderModel>())
                    .Build())
            {
                try
                {
                    consumer.Subscribe(EventConstants.Events.NewOrder);

                    while (!_cancelled)
                    {
                        ConsumeResult<string, OrderModel> message = consumer.Consume(default(CancellationToken));

                        // [DEMO] idempotence
                        if (_memoryCache.TryGetValue(message.Message.Key, out bool exists))
                        {
                            await _log.LogAsync($"Key {message.Message.Key} is duplicated, will skip for now");
                        }
                        else
                        {
                            await _log.LogAsync($"Consumer {idx} begins handle message {message.Message.Timestamp.UtcDateTime}");

                            using (IServiceScope scope = _serviceProvider.CreateScope())
                            {
                                await HandleNewOrderAsync(
                                    Guid.Parse(message.Message.Key),
                                    message.Message.Value,
                                    scope.ServiceProvider);
                            }

                            CacheKey(message.Message.Key);
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

        private async Task HandleNewOrderAsync(Guid key, OrderModel orderModel, IServiceProvider serviceProvider)
        {
            IOrderService orderService = serviceProvider.GetService<IOrderService>();

            double discount = await orderService.ApplyDiscountAsync(key, orderModel);

            await _log.LogAsync($"Finish applying discount of ${discount} for order {orderModel.Id}");
        }

        private void CacheKey(string key)
        {
            if (_memoryCache.Count > _memoryCacheOptions.Value.SizeLimit * 0.9)
            {
                _memoryCache.Compact(0.25);
            }

            // [DEMO] idempotence
            _memoryCache.Set(key, true,
                new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _log.LogAsync("[PROMOTION CALCULATOR]");

            for (int i = 0; i < _baseConfig.ConsumerCount; i++)
            {
                StartConsumerThread(i);
            }
        }
    }
}
