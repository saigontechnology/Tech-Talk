using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanningBook.DBEngine.Constants;

namespace PlanningBook.Identity.Infrastructure
{
    public static class Startup
    {
        public static void AddPBIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO-Improve: Use DBEngine in BuildingBlock for add db connection
            var test = $"{DBEngineConstants.RootConnectionString}:Identity{DBEngineConstants.dbConnectionStringPrefix}";
            services.AddDbContext<PBIdentityDbContext>(options => options.UseSqlServer(configuration[test]));
        }
    }
}
