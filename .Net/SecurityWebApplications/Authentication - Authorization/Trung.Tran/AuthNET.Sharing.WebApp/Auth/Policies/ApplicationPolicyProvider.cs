using AuthNET.Sharing.WebApp.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Auth.Policies
{
    // [Important] demo only: process policy names by syntax: policyA[_args];policyB[_args]...
    // So we don't have to create specific policy for each endpoint, just use the syntax with generic policies defined
    // Or we can change the requirements based on configuration in database, etc.
    public class ApplicationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _backupPolicyProvider;

        public ApplicationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies it should fall back to an alternate provider.
            _backupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _backupPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _backupPolicyProvider.GetFallbackPolicyAsync();

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyId)
        {
            // [Important] demo only: access configuration database
            if (!PolicyConfig.Map.TryGetValue(policyId, out var policyConfig))
            {
                var policy = await _backupPolicyProvider.GetPolicyAsync(policyId);
                return policy;
            }

            var policyEntries = policyConfig.Split(';', StringSplitOptions.RemoveEmptyEntries);

            var policyBuilder = new AuthorizationPolicyBuilder();

            foreach (var policyEntry in policyEntries)
            {
                var policyParts = policyEntry.Split('_', StringSplitOptions.RemoveEmptyEntries);
                var policyName = policyParts[0];

                switch (policyName)
                {
                    case AuthConstants.Policies.UserNameContains:
                        {
                            if (policyParts.Length != 2) throw new ArgumentException();
                            policyBuilder.AddRequirements(new UserNameContainsRequirement(policyParts[1]));
                            break;
                        }
                    case AuthConstants.Policies.HasRole:
                        {
                            if (policyParts.Length < 2) throw new ArgumentException();
                            policyBuilder.RequireRole(policyParts.Skip(1));
                            break;
                        }
                    default:
                        {
                            var policy = await _backupPolicyProvider.GetPolicyAsync(policyName);
                            policyBuilder = policyBuilder.Combine(policy);
                            break;
                        }
                }
            }

            return policyBuilder.Build();
        }
    }
}
