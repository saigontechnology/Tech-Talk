using AuthNET.Sharing.WebApp.Auth.Schemes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [Authorize] // [Important] wide access, can not change vice versa
    public class AuthenticationController : ControllerBase
    {
        // [Important] narrow access
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("token-based")]
        public string AccessTokenBased()
        {
            return "You passed token-based authentication";
        }

        [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("basic")]
        public string AccessBasic()
        {
            return "You passed basic authentication";
        }

        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("api-key")]
        public string AccessApiKey()
        {
            return "You passed API key authentication";
        }

        [AllowAnonymous]
        [HttpGet("anonymous")]
        public string AccessAnonymous()
        {
            return "You access this endpoint as anonymous";
        }
    }
}
