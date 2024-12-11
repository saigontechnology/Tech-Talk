using Microsoft.AspNetCore.Identity;
using PlanningBook.Identity.Infrastructure.Enums;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public RoleType RoleType { get; set; }
        public RoleAppliedEntity AppliedEntity { get; set; }
    }
}
