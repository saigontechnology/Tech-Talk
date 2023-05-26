using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TAuth.Resource.Cross;
using TAuth.Resource.Cross.Models.User;
using TAuth.ResourceAPI.Entities;

namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ResourceContext _context;

        public UsersController(ResourceContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var subject = User.FindFirst(JwtClaimTypes.Subject).Value;
            var claims = await _context.UserClaims.Where(uc => uc.UserId == subject)
                .Select(c => new UserProfileItem
                {
                    Type = c.ClaimType,
                    Value = c.ClaimValue
                }).ToArrayAsync();

            if (claims.Length == 0)
            {
                var roleClaim = new ApplicationUserClaim
                {
                    ClaimType = JwtClaimTypes.Role,
                    ClaimValue = RoleNames.NormalUser,
                    UserId = subject
                };

                _context.UserClaims.Add(roleClaim);

                await _context.SaveChangesAsync();

                claims = new[]
                {
                    new UserProfileItem
                    {
                        Type = roleClaim.ClaimType,
                        Value = roleClaim.ClaimValue
                    }
                };
            }

            return Ok(claims);
        }
    }
}
