using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TStore.SystemApi.Services;

namespace TStore.SystemApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost app = CreateHostBuilder(args).Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                await SetupMessageBrokerAsync(provider);
            }

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task SetupMessageBrokerAsync(IServiceProvider provider)
        {
            IMessageBrokerService messageBrokerService = provider.GetService<IMessageBrokerService>();

            await messageBrokerService.InitializeTopicsAsync();

            await messageBrokerService.InitializeAclsAsync();
        }
    }
}
