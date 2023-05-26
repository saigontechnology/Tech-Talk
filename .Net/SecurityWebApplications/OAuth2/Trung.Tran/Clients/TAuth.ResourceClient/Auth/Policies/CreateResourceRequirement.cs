using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.Resource.Cross;

namespace TAuth.ResourceClient.Auth.Policies
{
    public class CreateResourceRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> AllowedCountries { get; set; }
    }

    public class ResourceCreatorLocationHandler : AuthorizationHandler<CreateResourceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateResourceRequirement requirement)
        {
            if (context.HasSucceeded) return Task.CompletedTask;

            var addressClaim = context.User.FindFirst(JwtClaimTypes.Address);

            if (addressClaim == null) return Task.CompletedTask;

            var addressVal = addressClaim.Value;
            var addressObj = JsonConvert.DeserializeAnonymousType(addressVal, new
            {
                street_address = "",
                locality = "",
                postal_code = 0,
                country = ""
            });

            if (requirement.AllowedCountries?.Contains(addressObj.country) == true)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class AdminCreateResourceHandler : AuthorizationHandler<CreateResourceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateResourceRequirement requirement)
        {
            if (context.HasSucceeded) return Task.CompletedTask;

            if (context.User.IsInRole(RoleNames.Administrator))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
