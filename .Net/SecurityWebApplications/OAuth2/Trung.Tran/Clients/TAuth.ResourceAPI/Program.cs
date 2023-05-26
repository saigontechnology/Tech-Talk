using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;
using TAuth.Resource.Cross;
using TAuth.ResourceAPI.Entities;

namespace TAuth.ResourceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InitAsync(host).Wait();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task InitAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var dbContext = provider.GetRequiredService<ResourceContext>();

                var isInit = !(await dbContext.Database.GetAppliedMigrationsAsync()).Any();

                await dbContext.Database.MigrateAsync();

                if (isInit)
                {
                    await dbContext.Resources.AddRangeAsync(
                        new ResourceEntity
                        {
                            Name = "Sample Resource 1",
                            OwnerId = "1",
                        },
                        new ResourceEntity
                        {
                            Name = "Sample Resource 2",
                            OwnerId = "2"
                        });

                    await dbContext.UserClaims.AddRangeAsync(
                        new ApplicationUserClaim
                        {
                            ClaimType = JwtClaimTypes.Role,
                            ClaimValue = RoleNames.Administrator,
                            UserId = "1"
                        });

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
