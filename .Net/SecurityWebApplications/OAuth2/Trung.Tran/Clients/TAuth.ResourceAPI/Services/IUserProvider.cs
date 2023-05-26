using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TAuth.ResourceAPI.Services
{
    public interface IUserProvider
    {
        public string CurrentUserId { get; }
        public ClaimsPrincipal CurrentPrincipal { get; }
    }

    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null) return default;

                var subject = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);

                return subject;
            }
        }

        public ClaimsPrincipal CurrentPrincipal => _httpContextAccessor.HttpContext.User;
    }
}
