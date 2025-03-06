using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.Database.Core
{
    public static class DBEngineConfig
    {
        public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
