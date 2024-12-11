using Microsoft.AspNetCore.Identity;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class AccountClaim : IdentityUserClaim<Guid>
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}
