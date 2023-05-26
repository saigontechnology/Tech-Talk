using System.Collections.Generic;

namespace AuthNET.Sharing.WebApp.Models
{
    public class DataSnapshotModel
    {
        public IEnumerable<AppUserModel> Users { get; set; }
    }
}
