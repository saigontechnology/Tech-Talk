using Dumpify;
using efcore_demos.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Runner.Build((services, configuration) =>
{
    string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
    connectionString.Dump("SQL Server Connection String");
    services.AddDbContext<DemoDbContext>(s => s.UseSqlServer(connectionString));

    connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString_Second);
    connectionString.Dump("SQL Server Secondary Connection String");
    services.AddDbContext<DemoCompiledDbContext>(s => s.UseSqlServer(connectionString));
});

Runner.Execute();