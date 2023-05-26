using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.SSOClient
{
    public static class AuthConstants
    {
        public static class AuthenticationSchemes
        {
            public const string ExtraScheme = nameof(ExtraScheme);
        }

        public static class Cookie
        {
            public const string ExtraSchemeCookieName = "SSO.ExtraIdentity";
        }
    }
}
