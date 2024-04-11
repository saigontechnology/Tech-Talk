
namespace efcore_demos.Pieces;
internal class RawSqlUnmappedTypes : IExample
{
    private readonly DemoDbContext _dbContext;

    public RawSqlUnmappedTypes(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var orderInfo = await _dbContext.Database.SqlQuery<MyOrderInfo>($"SELECT * FROM v_OrderInfo_Top_10")
            .ToListAsync(cancellationToken);

        orderInfo.Dump();

        var orderInfo2 = await _dbContext.Database
            .SqlQueryRaw<MyOrderInfo>(
                "SELECT Code, '' as Name, 0 as Quantity, BillingAddress_City AS City, BillingAddress_Country as Country, BillingAddress_Postcode as Postcode, BillingAddress_Street AS Street FROM Orders"
            )
            .Where(x => x.Code.Contains("5"))
            .Take(2)
            .ToListAsync(cancellationToken);

        orderInfo2.Dump();
    }
}

internal class MyOrderInfo
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}
