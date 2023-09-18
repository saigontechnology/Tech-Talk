using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Api.Data.Entities;

namespace ProductCatalog.Api.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Set seed data
        builder.HasData(SeedData());
    }

    private static IEnumerable<Category> SeedData()
    {
        var categories = new List<Category>()
        {
            new Category()
            {
                Id = new Guid("cfe1306d-4b05-497e-991d-ffc5d4de84e1"),
                Name = "SARMs",
                Description = "Use for oral",
                CreatedBy = "SYSTEM",
                CreatedDate = new DateTime(2022, 08, 01)
            },
            new Category()
            {
                Id = new Guid("afd38cec-ef8b-44f6-b41e-694fc9c0e590"),
                Name = "Injection",
                Description = "Use for injection",
                CreatedBy = "SYSTEM",
                CreatedDate = new DateTime(2022, 08, 01)
            }
        };

        return categories;
    }
}