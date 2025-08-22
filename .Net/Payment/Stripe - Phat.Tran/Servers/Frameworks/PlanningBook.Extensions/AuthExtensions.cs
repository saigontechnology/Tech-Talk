using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PlanningBook.Extensions
{
    public static class AuthExtensions
    {
        public static Guid? GetCurrentAccountId(this ClaimsPrincipal account)
        {
            var userId = account.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrWhiteSpace(userId) ? null : Guid.Parse(userId);
        }

        public static string? GetCurrentJwtToken(this HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            return authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
        }
    }
}
