using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Persistence
{
    public class PermissionGroupRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PermissionGroupId { get; set; }
        public string Permission { get; set; }

        public virtual PermissionGroup PermissionGroup { get; set; }
    }
}
