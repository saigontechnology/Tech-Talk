using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TStore.Shared.Persistence;

namespace TStore.SaleApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost app = CreateHostBuilder(args).Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                await MigrateDatabaseAsync(provider);
            }

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task MigrateDatabaseAsync(IServiceProvider provider)
        {
            TStoreContext storeContext = provider.GetService<TStoreContext>();
            await storeContext.Database.MigrateAsync();
        }
    }
}
