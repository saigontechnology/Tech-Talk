using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;

namespace TStore.Consumers.PromotionCalculator
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

                    services.RegisterTStoreCommonServices(
                        SystemConstants.ServiceIds.PromotionCalculator,
                        configuration.GetConnectionString("TStore"));

                    services.Configure<MemoryCacheOptions>(opt =>
                    {
                        opt.SizeLimit = 1000;
                    });
                });
    }
}
