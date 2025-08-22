using PlanningBook.Domain;

namespace PlanningBook.Themes.Infrastructure.Entities
{
    public class StripeCustomer : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public string StripeCustomerId { get; set; }
        public string? StripePaymentMethodId { get; set; }
    }
}
