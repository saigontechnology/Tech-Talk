using PlanningBook.Domain;
using PlanningBook.Themes.Infrastructure.Entities.Enums;

namespace PlanningBook.Themes.Infrastructure.Entities
{
    public class Product : EntityBase<Guid>
    {
        // Demo Only
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; }
        public string StripeId { get; set; }
        public string StripePriceId { get; set; }
    }
}
