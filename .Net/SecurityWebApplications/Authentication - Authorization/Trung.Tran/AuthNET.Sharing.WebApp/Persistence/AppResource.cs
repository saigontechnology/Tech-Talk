using System;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class AppResource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual AppUser User { get; set; }
    }
}
