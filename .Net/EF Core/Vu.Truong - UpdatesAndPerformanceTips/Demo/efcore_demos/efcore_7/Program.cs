using efcore_demos.Constants;
using efcore_demos.DataAccess.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Runner.Build((services, configuration) =>
{
    string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
    connectionString.Dump("SQL Server Connection String");

    services.AddSingleton<SetRetrievedInterceptor>();
    services.AddSingleton<SetAuditInfoInterceptor>();

    services.AddDbContext<DemoDbContext>(s =>
    {
        s.UseSqlServer(connectionString);
        s.LogTo(message =>
        {
            if (message.Contains("Executing DbCommand"))
                Console.WriteLine(message);
        });
    });
});

await Runner.ExecuteAsync();