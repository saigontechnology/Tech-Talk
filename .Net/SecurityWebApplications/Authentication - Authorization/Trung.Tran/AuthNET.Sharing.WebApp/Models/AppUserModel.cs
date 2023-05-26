using System.Collections.Generic;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Models
{
    public class AppUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }

        public virtual IEnumerable<AppResourceModel> Resources { get; set; }
        public virtual IEnumerable<string> Roles { get; set; }
        public virtual IEnumerable<string> FinalPermissions => UserPermissions.Concat(GroupPermissions).Distinct();
        public virtual IEnumerable<string> PermissionGroups { get; set; }
        public virtual IEnumerable<string> UserPermissions { get; set; }
        public virtual IEnumerable<string> GroupPermissions { get; set; }
    }
}
