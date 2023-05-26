using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TStore.Shared.Constants;
using TStore.Shared.Services;

namespace TStore.Consumers.ExternalProductSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddHostedService<Worker>();

                    services.AddSingleton<IRealtimeNotiService, RealtimeNotiService>();

                    services.AddSingleton<IKafkaProducerManager, KafkaProducerManager>();

                    services.AddSingleton<IApplicationLog>(p =>
                    {
                        IRealtimeNotiService notiService = p.GetRequiredService<IRealtimeNotiService>();
                        return new ApplicationLog(SystemConstants.ServiceIds.ExternalProductSync, notiService);
                    });
                });
    }
}
