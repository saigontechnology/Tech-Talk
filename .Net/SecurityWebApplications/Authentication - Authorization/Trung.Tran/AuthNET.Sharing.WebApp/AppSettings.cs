using System.Collections.Generic;

namespace AuthNET.Sharing.WebApp
{
    public class AppSettings
    {
        public string JwtIssuer { get; set; }
        public IEnumerable<string> JwtAudiences { get; set; }
        public string JwtSecretKey { get; set; }
    }
}
