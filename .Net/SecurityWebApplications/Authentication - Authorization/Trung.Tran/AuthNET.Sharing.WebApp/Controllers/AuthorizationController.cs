using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorizationController : ControllerBase
    {
        [Authorize(AuthConstants.Policies.UserNameContains_A)]
        [HttpGet("username-contains-A")]
        public string UserNameContains_A()
        {
            return "PASSED";
        }

        [Authorize(AuthConstants.Policies.UserNameContains_B)]
        [HttpGet("username-contains-B")]
        public string UserNameContains_B()
        {
            return "PASSED";
        }

        [Authorize(AuthConstants.Policies.CanGetSpecial1)]
        [HttpGet("special-1")]
        public string Special1()
        {
            return "PASSED";
        }

        [Authorize(AuthConstants.Policies.CanGetSpecial2)]
        [HttpGet("special-2")]
        public string Special2()
        {
            return "PASSED";
        }
    }
}
