using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthNET.Sharing.WebApp
{
    public static class AuthConstants
    {
        public static class AuthenticationSchemes
        {
            public const string OAuth2 = nameof(OAuth2);
            public const string Facebook = nameof(Facebook);
            public const string Google = nameof(Google);
            public const string ExternalCookies = nameof(ExternalCookies);
        }

        public static class RoleNames
        {
            // [Important] And, Or condition
            public const string Administrator = nameof(Administrator);
            public const string Employee = nameof(Employee);
        }

        public static class Permissions
        {
            public const string FullAccess = "full";
            public const string Read = "read";
            public const string Write = "write";
        }

        public static class Policies
        {
            public const string CanAccessAdminArea = nameof(CanAccessAdminArea);
            public const string CanReadResource = nameof(CanReadResource);
            public const string CanWriteResource = nameof(CanWriteResource);
            public const string CanManageResource = nameof(CanManageResource);
            public const string SingleRoleOnly = nameof(SingleRoleOnly);

            public const string UserNameContains = nameof(UserNameContains);
            public const string UserNameContains_A = nameof(UserNameContains_A);
            public const string UserNameContains_B = nameof(UserNameContains_B);

            public const string HasRole = nameof(HasRole);

            public const string CanGetSpecial1 = nameof(CanGetSpecial1);
            public const string CanGetSpecial2 = nameof(CanGetSpecial2);
        }

        public static class AppClaimTypes
        {
            public const string UserName = "username";
            public const string Permission = "permission";
            public const string Subject = "sub";
        }

        public static class Jwt
        {
            public static readonly TokenValidationParameters DefaultTokenParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Startup.AppSettings.JwtIssuer,
                ValidAudiences = Startup.AppSettings.JwtAudiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Startup.AppSettings.JwtSecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        }
    }

    public static class PolicyConfig
    {
        public static IDictionary<string, string> Map = new Dictionary<string, string>()
        {
            [AuthConstants.Policies.CanGetSpecial1] = $"{AuthConstants.Policies.UserNameContains}_A;" +
                            $"{AuthConstants.Policies.UserNameContains}_user;" +
                            $"{AuthConstants.Policies.HasRole}_{AuthConstants.RoleNames.Employee}",

            [AuthConstants.Policies.CanGetSpecial2] = $"{AuthConstants.Policies.UserNameContains}_B;" +
                            $"{AuthConstants.Policies.UserNameContains}_user;" +
                            $"{AuthConstants.Policies.HasRole}_{AuthConstants.RoleNames.Employee}" +
                            $"{AuthConstants.Policies.HasRole}_{AuthConstants.RoleNames.Administrator}"
        };
    }
}
