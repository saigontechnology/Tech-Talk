using System;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class AppUserPermission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string Permission { get; set; }

        public virtual AppUser User { get; set; }
    }
}
