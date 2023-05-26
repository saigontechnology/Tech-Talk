using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TAuth.ResourceAPI.Auth.Policies
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class IsOwnerHandler : AuthorizationHandler<IsOwnerRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement, int ownerId)
        {
            var userId = context.User.FindFirst(JwtClaimTypes.Subject).Value;

            if (userId == $"{ownerId}")
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
