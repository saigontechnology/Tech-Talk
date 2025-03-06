using BuildingBlock.Database.Constants;
using BuildingBlock.Database.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure
{
    public static class Startup
    {
        public static void AddIdentityServiceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration[$"{DBEngineConstants.RootConnectionString}:Identity{DBEngineConstants.dbConnectionStringPrefix}"];
            services.AddSqlServer<IdentityServiceDbContext>(connectionString);
        }
    }
}
