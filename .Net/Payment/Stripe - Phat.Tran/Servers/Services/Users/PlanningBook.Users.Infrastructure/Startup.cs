using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanningBook.DBEngine.Constants;

namespace PlanningBook.Users.Infrastructure
{
    public static class Startup
    {
        public static void AddPBPersonDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO-Improve: Use DBEngine in BuildingBlock for add db connection
            var test = $"{DBEngineConstants.RootConnectionString}:Person{DBEngineConstants.dbConnectionStringPrefix}";
            services.AddDbContext<PBPersonDbContext>(options => options.UseSqlServer(configuration[test]));
        }
    }
}
