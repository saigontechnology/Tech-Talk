using System;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class UserPermissionGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public Guid GroupId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual PermissionGroup PermissionGroup { get; set; }
    }
}
