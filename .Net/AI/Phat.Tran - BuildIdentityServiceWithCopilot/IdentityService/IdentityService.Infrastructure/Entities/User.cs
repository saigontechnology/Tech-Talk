using Framework.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace IdentityService.Infrastructure.Entities
{
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
    }
}
