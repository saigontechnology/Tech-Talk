using efcore_performances.Constants;
using efcore_performances.DataAccess.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Linq.Expressions;

namespace efcore_performances.Pieces.Tests;

internal class DbContextPoolingTest
{
    const int MaxThreads = 64;
    const int Seconds = 10;
    static long _requestsProcessed;
    static DbContextType dbContextType = DbContextType.Normal;
    static readonly Func<DemoDbContext, CancellationToken, Task<UserEntity>> firstUserQuery;

    static DbContextPoolingTest()
    {
        Expression<Func<DemoDbContext, CancellationToken, UserEntity>> firstUserExpr = (context, cancellationToken) => context.Users.First();
        firstUserQuery = EF.CompileAsyncQuery(firstUserExpr);
    }

    public static async Task Run(DbContextType dbType = DbContextType.Normal, bool useCompiledQuery = false, int threads = MaxThreads)
    {
        var container = Container.GetInstance(Guid.NewGuid().ToString());

        dbContextType = dbType;
        DemoDbContext.InstanceCount = 0;
        DemoDbContext.DisposedCount = 0;
        _requestsProcessed = 0;

        Console.WriteLine();
        Console.WriteLine("=================");
        Console.WriteLine($"Run Test DbContext, Type = {dbContextType}");

        container.Build((services, configuration) =>
        {
            container.AddDbContext(services, configuration, false, dbContextType);
        });

        CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(Seconds));

        await Task.WhenAll(
            Enumerable
                .Range(0, threads + 1)
                .Select(i =>
                {
                    if (i == 0)
                    {
                        return MonitorResults(cancellationTokenSource.Token);
                    }

                    return SimulateRequestsAsync(container.ServiceProvider, useCompiledQuery, cancellationTokenSource.Token);
                })
        );

        container.Dispose();
    }

    private static async Task Action(DemoDbContext dbContext, bool useCompiled = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (dbContext is not null)
            {
                if (!useCompiled)
                {
                    await dbContext.Users.FirstAsync(cancellationToken);
                }
                else
                {
                    await firstUserQuery(dbContext, cancellationToken);
                }
            }
        }
        catch { }
    }

    private static async Task SimulateRequestsAsync(IServiceProvider serviceProvider, bool useCompiledQuery, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                if (dbContextType == DbContextType.Normal || dbContextType == DbContextType.Pooling)
                {
                    using var dbContext = serviceScope.ServiceProvider.GetService<DemoDbContext>();
                    await Action(dbContext, useCompiledQuery, cancellationToken);
                }
                else
                {
                    var dbContextFactory = serviceScope.ServiceProvider.GetService<IDbContextFactory<DemoDbContext>>();
                    using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                    await Action(dbContext, useCompiledQuery, cancellationToken);
                }
            }

            Interlocked.Increment(ref _requestsProcessed);
        }
    }

    private static async Task MonitorResults(CancellationToken cancellationToken = default)
    {
        var lastInstanceCount = 0L;
        var lastDisposedCount = 0L;
        var lastRequestCount = 0L;
        var lastElapsed = TimeSpan.Zero;

        Stopwatch stopwatch = Stopwatch.StartNew();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                var instanceCount = DemoDbContext.InstanceCount;
                var disposedCount = DemoDbContext.DisposedCount;
                var requestCount = _requestsProcessed;
                var elapsed = stopwatch.Elapsed;
                var currentElapsed = elapsed - lastElapsed;
                var currentRequests = requestCount - lastRequestCount;

                Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss.fff}] "
                    + $"Context creations/second: {instanceCount - lastInstanceCount} | "
                    + $"Context disposed/second: {disposedCount - lastDisposedCount} | "
                    + $"Requests/second: {Math.Round(currentRequests / currentElapsed.TotalSeconds)}");

                lastInstanceCount = instanceCount;
                lastDisposedCount = disposedCount;
                lastRequestCount = requestCount;
                lastElapsed = elapsed;
            }
            catch
            {
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Total context creations: {DemoDbContext.InstanceCount}");
        Console.WriteLine($"Total context disposed: {DemoDbContext.DisposedCount}");
        Console.WriteLine(
            $"Requests per second:     {Math.Round(_requestsProcessed / stopwatch.Elapsed.TotalSeconds)}");

        stopwatch.Stop();
    }
}