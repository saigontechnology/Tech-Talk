using AuthNET.Sharing.WebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [Route("api/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public OAuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// [Important] Get token for demo only, should use library instead
        /// </summary>
        [HttpPost("token")]
        public async Task<IActionResult> GetAccessToken([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _identityService.AuthenticateAsync(model.Username, model.Password);

            if (user == null) return Unauthorized();

            var claimsPrincipal = await _identityService.GetUserPrincipalAsync(user, JwtBearerDefaults.AuthenticationScheme);

            var tokenResponse = GenerateTokenResponse(claimsPrincipal);
            return Ok(tokenResponse);
        }

        private string GenerateTokenResponse(ClaimsPrincipal principal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.Default.GetBytes(Startup.AppSettings.JwtSecretKey);
            var issuer = Startup.AppSettings.JwtIssuer;
            var identity = principal.Identity as ClaimsIdentity;
            var audClaims = Startup.AppSettings.JwtAudiences
                .Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)).ToArray();

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, principal.Identity.Name));
            identity.AddClaims(audClaims);

            var utcNow = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                //Audience = audience,
                Subject = identity,
                IssuedAt = utcNow,
                Expires = utcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = utcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
