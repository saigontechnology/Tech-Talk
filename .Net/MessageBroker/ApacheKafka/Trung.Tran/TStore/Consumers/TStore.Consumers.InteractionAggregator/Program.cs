using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;

namespace TStore.Consumers.InteractionAggregator
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

                    services.RegisterInteractionCommonServices(
                        SystemConstants.ServiceIds.InteractionAggreator,
                        configuration.GetConnectionString("TStoreInteraction"));
                });
    }
}
