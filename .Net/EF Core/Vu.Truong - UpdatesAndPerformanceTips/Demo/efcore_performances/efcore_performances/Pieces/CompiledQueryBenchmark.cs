using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace efcore_performances.Pieces;

public class CompiledQueryBenchmark
{
    private Container container;
    static Func<DemoDbContext, IAsyncEnumerable<ProductReportModel>> singleUserQuery;

    private DemoDbContext _dbContext;

    [Params(100, 1000, 10000)]
    public int Counter { get; set; }

    [GlobalSetup]
    public Task Setup()
    {
        container = Container.GetInstance(Guid.NewGuid().ToString());

        container.Build((services, configuration) =>
        {
            container.AddDbContext(services, configuration);
        });

        _dbContext = container.ServiceProvider.GetRequiredService<DemoDbContext>();

        singleUserQuery = EF.CompileAsyncQuery((DemoDbContext db)
            => db.Products
                .GroupJoin(
                    db.Orders
                        .Select(o => new OrderReportModel
                        {
                            ProductId = o.ProductId,
                            Code = o.Code
                        }),
                    p => p.Id,
                    o => o.ProductId,
                    (p, o) => new ProductReportModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Orders = o.ToList()
                    })
        );

        return Task.CompletedTask;
    }

    [GlobalCleanup]
    public Task Cleanup()
    {
        container.Dispose();

        return Task.CompletedTask;
    }

    private async Task<int> DoRegularQuery()
    {
        var size = 0;

        var products = _dbContext.Products
               .GroupJoin(
                   _dbContext.Orders
                       .Select(o => new OrderReportModel
                       {
                           ProductId = o.ProductId,
                           Code = o.Code
                       }),
                   p => p.Id,
                   o => o.ProductId,
                   (p, o) => new ProductReportModel
                   {
                       Id = p.Id,
                       Name = p.Name,
                       Quantity = p.Quantity,
                       Orders = o.ToList()
                   })
               .AsAsyncEnumerable();

        await foreach (var product in products)
        {
            size += product.Quantity;
        }

        return size;
    }

    [Benchmark]
    public async Task<int> RegularQuery()
    {
        var result = 0;
        for (int i = 0; i < Counter; i++)
        {
            result = await DoRegularQuery();
        }

        return result;
    }

    private async Task<int> DoCompiledQuery()
    {
        var size = 0;

        await foreach (var product in singleUserQuery(_dbContext))
        {
            size += product.Quantity;
        }

        return size;
    }

    [Benchmark]
    public async Task<int> CompiledQuery()
    {
        var result = 0;
        for (int i = 0; i < Counter; i++)
        {
            result = await DoCompiledQuery();
        }

        return result;
    }
}

internal class ProductReportModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public List<OrderReportModel> Orders { get; set; }
}

internal class OrderReportModel
{
    public Guid ProductId { get; set; }
    public List<InvoiceEntity> Invoices { get; set; }
    public string Code { get; set; }
}

internal class InvoiceReportModel
{
    public string InvoiceCode { get; set; }
}