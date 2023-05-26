using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TAuth.Resource.Cross;

namespace TAuth.ResourceAPI.Auth.Policies
{
    public class DeleteResourceRequirement : IAuthorizationRequirement
    {
        public string TestName { get; set; }
    }

    public class ResourceAuthorizationModel
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }

    public class DeleteTestResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            if (context.HasSucceeded) return Task.CompletedTask;

            if (resource.Name == requirement.TestName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class DeleteOwnedResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            if (context.HasSucceeded) return Task.CompletedTask;

            var userId = context.User.FindFirst(JwtClaimTypes.Subject).Value;

            if (userId == $"{resource.OwnerId}")
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class AdminDeleteResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            if (context.HasSucceeded) return Task.CompletedTask;

            if (context.User.IsInRole(RoleNames.Administrator))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
