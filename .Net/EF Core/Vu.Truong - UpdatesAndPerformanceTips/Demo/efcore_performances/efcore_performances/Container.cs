using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace efcore_performances;
internal class Container
{
    private readonly static Dictionary<string, Container> Instances = new();
    private const string Default_Identify = "Main_Container";
    private bool _disposed = false;
    private readonly string _identifier;

    private Container(string identifier)
    {
        _identifier = identifier;
    }

    public static Container GetInstance(string identify = null)
    {
        if (string.IsNullOrEmpty(identify))
        {
            identify = Default_Identify;
        }

        Instances.TryGetValue(identify, out var instance);

        if (instance is null)
        {
            instance = new Container(identify);
            Instances.TryAdd(identify, instance);
        }

        return instance;
    }

    public ServiceProvider ServiceProvider { get; set; }

    public void Build(Action<IServiceCollection, IConfiguration> startupAction = null)
    {
        var serviceCollection = new ServiceCollection()
            .AddLogging();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        serviceCollection.AddSingleton<IConfiguration>(configuration);

        if (startupAction is not null)
        {
            startupAction(serviceCollection, configuration);
        }

        ServiceProvider = serviceCollection
                .BuildServiceProvider();
    }

    public void AddDbContext(IServiceCollection services, IConfiguration configuration, bool enableLog = true, DbContextType contextType = DbContextType.Normal)
    {
        services.AddSingleton<SetAuditInfoInterceptor>();

        var connectionString = configuration.GetConnectionString(Commons.SqlServer_ConString);

        void configSqlServer(IServiceProvider services, DbContextOptionsBuilder opt)
        {
            opt.UseSqlServer(connectionString);

            opt.LogTo(message =>
            {
                if (!enableLog)
                {
                    return;
                }

                if (message.Contains("Executing DbCommand"))
                {
                    Console.WriteLine();
                    Console.WriteLine();
#if NET8_0_OR_GREATER
                    Console.WriteLine("===========NET8.0===========");
#elif NET6_0_OR_GREATER
                    Console.WriteLine("===========NET6.0===========");
#endif
                    Console.WriteLine(message);
#if NET8_0_OR_GREATER
                    Console.WriteLine("===========NET8.0===========");
#elif NET6_0_OR_GREATER
                    Console.WriteLine("===========NET6.0===========");
#endif
                    Console.WriteLine();
                    Console.WriteLine();
                }
            });

            var auditInterceptors = services.GetRequiredService<SetAuditInfoInterceptor>();
            opt.AddInterceptors(auditInterceptors);
        }

        switch (contextType)
        {
            case DbContextType.Normal:
                services.AddDbContext<DemoDbContext>(configSqlServer);
                break;
            case DbContextType.Pooling:
                services.AddDbContextPool<DemoDbContext>(configSqlServer);
                break;
            case DbContextType.Factory:
                services.AddDbContextFactory<DemoDbContext>(configSqlServer);
                break;
            case DbContextType.PooledFactory:
                services.AddPooledDbContextFactory<DemoDbContext>(configSqlServer);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (ServiceProvider is not null)
        {
            ServiceProvider.Dispose();
            _disposed = true;
            Instances.Remove(_identifier);
        }
    }
}
