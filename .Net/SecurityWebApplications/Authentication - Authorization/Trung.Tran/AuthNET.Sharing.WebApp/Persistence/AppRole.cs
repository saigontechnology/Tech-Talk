using System.Collections.Generic;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class AppRole : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
