using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;
using PlanningBook.Themes.Infrastructure.Entities.Enums;

namespace PlanningBook.Themes.Infrastructure.Entities.Configurations
{
    public class ProductConfiguration : BaseRelationDbEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.HasData(
                // Subscriptions
                new Product()
                {
                    Id = new Guid("1fc13c2a-27e8-45cb-95dd-9dfd924db840"),
                    StripeId = "prod_RGHOXdDumkw4WR",
                    Name = "Lifetime Plan",
                    Price = 710,
                    StripePriceId = "price_1QNkpFDkmueHRg9S4XBlzXWg",
                    ProductType = ProductType.SubcriptionPlan
                },
                new Product()
                {
                    Id = new Guid("b677c4c6-669b-43ca-9897-83b5cb1c0cd9"),
                    StripeId = "prod_RGHNxfzXdhiDTx",
                    Name = "Elite Plan",
                    Price = 294,
                    StripePriceId = "price_1QNkobDkmueHRg9SOjsRIR9U",
                    ProductType = ProductType.SubcriptionPlan
                },
                new Product()
                {
                    Id = new Guid("371cffea-2b1e-4c4d-aec8-cffd1ae43fef"),
                    StripeId = "prod_RGHLOw7fxZK5kZ",
                    Name = "Basic Plan",
                    Price = 150,
                    StripePriceId = "price_1QNkmkDkmueHRg9S9kzCsJK0",
                    ProductType = ProductType.SubcriptionPlan
                },
                new Product()
                {
                    Id = new Guid("835bc37f-8891-48da-9f01-4bfcbf50ab13"),
                    StripeId = "prod_RICKs6KQp8yTZN",
                    Name = "Theme One",
                    Price = 150,
                    ProductType = ProductType.Theme,
                    StripePriceId = "price_1QPbvhDkmueHRg9SYxUpGaE4"
                },
                new Product()
                {
                    Id = new Guid("27784869-292e-47e8-be5f-311d7a4aaf14"),
                    StripeId = "prod_RICKg7t3XaDv6z",
                    Name = "Theme Two",
                    Price = 250,
                    ProductType = ProductType.Theme,
                    StripePriceId = "price_1QPbw8DkmueHRg9SEP1p1iMK"
                },
                new Product()
                {
                    Id = new Guid("eea6122c-c5d8-40f1-a44a-766008241255"),
                    StripeId = "prod_RICLOCfsBhGqge",
                    Name = "Theme Three",
                    Price = 400,
                    ProductType = ProductType.Theme,
                    StripePriceId = "price_1QPbwlDkmueHRg9Smb83pb2L"
                }
                );
        }
    }
}
