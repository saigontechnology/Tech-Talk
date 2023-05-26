using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TStore.InteractionApi.Consumers;
using TStore.Shared.Constants;
using TStore.Shared.Persistence;
using TStore.Shared.Services;

namespace TStore.InteractionApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost app = CreateHostBuilder(args).Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                await InitNotiAsync(provider);
                await MigrateDatabaseAsync(provider);
                StartConsumers(app.Services);
            }

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task InitNotiAsync(IServiceProvider provider)
        {
            IRealtimeNotiService noti = provider.GetService<IRealtimeNotiService>();
            await noti.NotifyLogAsync(SystemConstants.ServiceIds.InteractionApi, "Starting Interaction Api");
        }

        private static async Task MigrateDatabaseAsync(IServiceProvider provider)
        {
            InteractionContext interactionContext = provider.GetService<InteractionContext>();
            await interactionContext.Database.MigrateAsync();
        }

        private static void StartConsumers(IServiceProvider rootProvider)
        {
            IConfiguration configuration = rootProvider.GetService<IConfiguration>();
            bool saveByConsumers = configuration.GetValue<bool>("SaveByConsumers");

            if (!saveByConsumers)
            {
                return;
            }

            int numberOfConsumers = configuration.GetSection("SaveInteractionConsumerConfig").GetValue<int>("ConsumerCount");
            for (int i = 0; i < numberOfConsumers; i++)
            {
                ISaveInteractionConsumer saveInteractionConsumer = rootProvider.GetRequiredService<ISaveInteractionConsumer>();
                saveInteractionConsumer.StartListenThread(i);
            }
        }
    }
}
