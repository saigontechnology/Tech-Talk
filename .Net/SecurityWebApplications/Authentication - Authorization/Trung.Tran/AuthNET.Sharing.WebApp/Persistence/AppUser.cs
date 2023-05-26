using System.Collections.Generic;

namespace AuthNET.Sharing.WebApp.Persistence
{
    // [Important] can used IdentityFramework for easy user management
    public class AppUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; } // [Important] demo only, need hashing
        public string ApiKey { get; set; } // [Important] SendGrid for example

        public virtual ICollection<AppResource> Resources { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<AppUserPermission> UserPermissions { get; set; }
        public virtual ICollection<UserPermissionGroup> UserPermissionGroups { get; set; }
    }
}
