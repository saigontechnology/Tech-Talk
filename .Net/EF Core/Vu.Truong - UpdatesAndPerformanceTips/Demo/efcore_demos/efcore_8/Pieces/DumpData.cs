using System.Linq;

namespace efcore_demos.Pieces;
internal class DumpData : IExample
{
    private readonly DemoDbContext _dbContext;

    const int Rows = 100;

    public DumpData(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Dump data
        var products = Enumerable.Range(0, Rows)
                .Select(i => new ProductEntity
                {
                    Description = Faker.Company.Name(),
                    Name = Faker.Company.Name(),
                    Price = new Money(Faker.RandomNumber.Next(100, 1000), Faker.Enum.Random<Currency>()),
                    Quantity = Faker.RandomNumber.Next(50, 100)
                }).ToList();

        _dbContext.Products.AddRange(products);

        var orders = products
            .Select((p, i) =>
            {
                var address = EntityHelper.GenerateAddress();

                return new OrderEntity
                {
                    Code = $"Code {i}",
                    ProductId = p.Id,
                    DeliveryDetail = EntityHelper.GenerateDeliveryDetail(),
                    ShippingAddress = address,
                    BillingAddress = address,
                };
            })
            .ToList();

        _dbContext.Orders.AddRange(orders);

        _dbContext.Users.AddRange(
            Enumerable.Range(0, Rows)
                .Select(i => new UserEntity
                {
                    Email = Faker.Internet.Email(),
                    BirthDate = Faker.Identification.DateOfBirth(),
                    Password = Faker.Identification.UsPassportNumber(),
                    UserName = Faker.Internet.UserName(),
                    Address = EntityHelper.GenerateAddress(),
                    DaysVisited = [DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDayNumber(200)]
                })
        );

        var invoices = orders
            .Select((order, i) =>
            {
                var invoice = new InvoiceEntity
                {
                    InvoiceCode = $"Invoie Code {i}",
                    OrderCode = order.Code,
                    Addresses = new List<AddressEntity>
                    {
                        EntityHelper.GenerateAddress()
                    },
                    Contact = new ContactEntity
                    {
                        Address = order.BillingAddress,
                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last(),   
                        PhoneNumber = new PhoneNumberEntity(Faker.RandomNumber.Next(10, 99), Faker.RandomNumber.Next(100000000, 999999999))
                    }
                };

                return invoice;
            })
            .ToList();

        _dbContext.Invoices.AddRange(invoices);

        await _dbContext.Documents.AddRangeAsync(
            new DocumentEntity("Root", "This is root folder", DocumentBranchType.Folder, HierarchyId.Parse("/")),

            new DocumentEntity(Faker.Lorem.GetFirstWord(), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/1/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/1/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/1/1/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/1/1/2/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/1/1/3/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/1/1/4/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/1/2/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/1/2/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/1/2/1/1/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/2/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.Folder, HierarchyId.Parse("/2/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/2/1/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/2/1/2/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/2/1/3/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/2/1/4/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/3/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/3/1/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/3/1/2/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/3/1/3/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/3/1/4/")),

            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/4/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/4/1/1/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/4/1/2/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/4/1/3/")),
            new DocumentEntity(string.Join(' ', Faker.Lorem.Words(2)), Faker.Lorem.Sentence(5), DocumentBranchType.File, HierarchyId.Parse("/4/1/4/"))
        );


        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
