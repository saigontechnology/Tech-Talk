using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class PermissionGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string GroupName { get; set; }

        public virtual ICollection<PermissionGroupRecord> Records { get; set; }
    }
}
