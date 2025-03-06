using BuildingBlock.Database.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure
{
    public class IdentityServiceDbContextFactory : IDesignTimeDbContextFactory<IdentityServiceDbContext>
    {
        public IdentityServiceDbContext CreateDbContext(string[] args)
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
                ? args[0] : configuration[configurationPath];

            var optionsBuilder = new DbContextOptionsBuilder<IdentityServiceDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new IdentityServiceDbContext(optionsBuilder.Options);
        }
    }
}
