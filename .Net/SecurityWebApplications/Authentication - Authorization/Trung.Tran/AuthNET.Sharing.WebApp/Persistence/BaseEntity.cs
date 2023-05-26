using System;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTimeOffset LastChanged { get; set; } = DateTimeOffset.UtcNow;
    }
}
