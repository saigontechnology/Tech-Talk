using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;
using System.Threading;

namespace efcore_performances.Pieces;
internal class SavingChangeTest : IExample
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        BenchmarkDotNet.Running.BenchmarkRunner.Run<EFCorePerfBenchmark>();

        return Task.CompletedTask;
    }
}

[RankColumn]
[MinColumn]
[MaxColumn]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
public class EFCorePerfBenchmark
{
    private Container container;
    private Container pooledContainer;

    [Params(100, 1000)]
    public int Counter { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        container = Container.GetInstance(Guid.NewGuid().ToString());
        pooledContainer = Container.GetInstance(Guid.NewGuid().ToString());

        container.Build((services, configuration) =>
        {
            container.AddDbContext(services, configuration, false);
        });

        pooledContainer.Build((services, configuration) =>
        {
            pooledContainer.AddDbContext(services, configuration, false, DbContextType.PooledFactory);
        });
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        container.Dispose();
    }

    [Benchmark]
    public async Task<int> BulkUpdateAsync()
    {
        var dbContext = container.ServiceProvider.GetRequiredService<DemoDbContext>();

        UpdateToDb(dbContext);

        return await dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task<int> BulkUpdatePooledAsync()
    {
        var dbContextFactory = pooledContainer.ServiceProvider.GetService<IDbContextFactory<DemoDbContext>>();
        using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var now = DateTime.UtcNow;

        UpdateToDb(dbContext);

        return await dbContext.SaveChangesAsync();
    }

    private void UpdateToDb(DemoDbContext dbContext)
    {
        var now = DateTime.UtcNow;

#if NET7_0_OR_GREATER
        //dbContext.Products
        //    .ExecuteUpdate(x => x.SetProperty(s => s.Name, $"{now.ToFileTimeUtc()}"));

#endif

        var products = Enumerable.Range(0, Counter)
            .Select(x => new ProductEntity
            {
                Description = $"P {x}",
                Name = $"P N {x}",
                Quantity = x
            })
            .ToList();

        dbContext.Products.AddRange(products);
    }
}