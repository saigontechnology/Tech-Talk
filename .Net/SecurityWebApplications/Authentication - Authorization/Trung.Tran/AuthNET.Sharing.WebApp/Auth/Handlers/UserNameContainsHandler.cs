using AuthNET.Sharing.WebApp.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Auth.Handlers
{
    public class UserNameContainsHandler : AuthorizationHandler<UserNameContainsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNameContainsRequirement requirement)
        {
            var userName = context.User.FindFirstValue(AuthConstants.AppClaimTypes.UserName);

            if (userName?.Contains(requirement.Value) == true)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
