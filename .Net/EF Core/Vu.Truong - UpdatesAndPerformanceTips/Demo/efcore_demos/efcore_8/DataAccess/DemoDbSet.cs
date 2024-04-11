namespace efcore_demos.DataAccess;
internal partial class DemoDbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<DocumentEntity> Documents { get; set; }
}
