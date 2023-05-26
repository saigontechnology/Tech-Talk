using System;

namespace AuthNET.Sharing.WebApp.Models
{
    public class AppResourceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
