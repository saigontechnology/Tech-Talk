using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanningBook.DBEngine.Constants;

namespace PlanningBook.Themes.Infrastructure
{
    public static class Startup
    {
        public static void AddPBThemeDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO-Improve: Use DBEngine in BuildingBlock for add db connection
            var test = $"{DBEngineConstants.RootConnectionString}:Theme{DBEngineConstants.dbConnectionStringPrefix}";
            services.AddDbContext<PBThemeDbContext>(options => options.UseSqlServer(configuration[test]));
        }
    }
}
