using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TAuth.IDP.Models
{
    public class AppUser : IdentityUser
    {
        public bool Active { get; set; }

        public virtual ICollection<UserSecret> UserSecrets { get; set; }
    }
}
