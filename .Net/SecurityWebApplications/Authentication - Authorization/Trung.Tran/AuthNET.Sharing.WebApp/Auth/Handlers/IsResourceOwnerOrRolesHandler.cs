using AuthNET.Sharing.WebApp.Auth.Policies;
using AuthNET.Sharing.WebApp.Persistence;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Auth.Requirements
{
    // [Important] Why a seperate auth handler, why not placed inside business logic
    public class IsResourceOwnerOrRolesHandler : AuthorizationHandler<IsResourceOwnerOrRolesRequirement, CanManageResourceContext>
    {
        private readonly DataContext _dataContext;

        public IsResourceOwnerOrRolesHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsResourceOwnerOrRolesRequirement requirement,
            CanManageResourceContext resource)
        {
            var userId = context.User.Identity.Name;
            var ownerId = _dataContext.Resources.Where(r => r.Id == resource.ResourceId)
                .Select(r => r.UserId).FirstOrDefault();

            if (ownerId == null) return Task.CompletedTask;

            if (userId == ownerId || requirement.Roles.Any(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);

                // [Important] one handler can set the result of other requirements (if not fail)
                foreach (var pendingRequirement in context.PendingRequirements)
                    context.Succeed(pendingRequirement);
            }
            else
            {
                // [Important] if the requirement is essential, call Fail() to prevent access
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
