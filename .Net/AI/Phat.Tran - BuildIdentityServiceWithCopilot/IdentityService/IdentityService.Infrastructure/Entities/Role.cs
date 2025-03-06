using Framework.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Entities
{
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {
        public Role()
        {

        }

        public Role(string roleName) : base(roleName)
        {

        }
    }
}
