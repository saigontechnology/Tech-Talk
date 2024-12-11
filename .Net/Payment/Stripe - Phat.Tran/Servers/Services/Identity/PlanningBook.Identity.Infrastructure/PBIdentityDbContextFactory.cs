using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PlanningBook.DBEngine.Constants;

namespace PlanningBook.Identity.Infrastructure
{
    public class PBIdentityDbContextFactory : IDesignTimeDbContextFactory<PBIdentityDbContext>
    {
        public PBIdentityDbContext CreateDbContext(string[] args)
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Use the same connection string key as in Startup.AddPBIdentityDbContext
            var configurationPath = $"{DBEngineConstants.RootConnectionString}:Identity{DBEngineConstants.dbConnectionStringPrefix}";
            var connectionString = (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                            ? args[0]
                            : configuration[configurationPath];

            var optionsBuilder = new DbContextOptionsBuilder<PBIdentityDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new PBIdentityDbContext(optionsBuilder.Options);
        }
    }
}
