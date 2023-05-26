using System;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class AppUserRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
