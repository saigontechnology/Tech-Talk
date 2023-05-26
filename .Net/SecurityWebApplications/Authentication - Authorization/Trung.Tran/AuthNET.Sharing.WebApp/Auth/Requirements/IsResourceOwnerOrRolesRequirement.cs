using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace AuthNET.Sharing.WebApp.Auth.Requirements
{
    public class IsResourceOwnerOrRolesRequirement : IAuthorizationRequirement
    {
        public IsResourceOwnerOrRolesRequirement(params string[] roles)
        {
            Roles = roles;
        }

        public IEnumerable<string> Roles { get; set; }
    }
}
