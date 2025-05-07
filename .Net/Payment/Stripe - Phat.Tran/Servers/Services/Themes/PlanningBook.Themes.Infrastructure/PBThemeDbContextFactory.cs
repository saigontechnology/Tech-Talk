using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PlanningBook.DBEngine.Constants;

namespace PlanningBook.Themes.Infrastructure
{
    public class PBThemeDbContextFactory : IDesignTimeDbContextFactory<PBThemeDbContext>
    {
        public PBThemeDbContext CreateDbContext(string[] args)
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var configurationPath = $"{DBEngineConstants.RootConnectionString}:Theme{DBEngineConstants.dbConnectionStringPrefix}";
            var connectionString = (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                            ? args[0]
                            : configuration[configurationPath];

            var optionsBuilder = new DbContextOptionsBuilder<PBThemeDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new PBThemeDbContext(optionsBuilder.Options);
        }
    }
}
