using efcore_demos.Constants;
using efcore_demos.DataAccess.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Runner.Build((services, configuration) =>
{
    string connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);
    connectionString.Dump("SQL Server Connection String");

    services.AddSingleton<SetAuditInfoInterceptor>();

    services.AddDbContext<DemoDbContext>(s =>
    {
        s.UseSqlServer(connectionString, x => x.UseHierarchyId());
        s.LogTo(message =>
        {
            if (message.Contains("Executing DbCommand"))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("======================");
                Console.WriteLine(message);
                Console.WriteLine("======================");
                Console.WriteLine();
                Console.WriteLine();
            }
        });
    });
});

await Runner.ExecuteAsync();