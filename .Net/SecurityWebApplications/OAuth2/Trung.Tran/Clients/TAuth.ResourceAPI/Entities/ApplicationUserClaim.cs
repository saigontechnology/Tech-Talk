using Microsoft.AspNetCore.Identity;

namespace TAuth.ResourceAPI.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public ApplicationUserClaim()
        {
        }
    }
}
