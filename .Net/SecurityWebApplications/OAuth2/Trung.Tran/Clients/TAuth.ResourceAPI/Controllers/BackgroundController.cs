using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TAuth.Cross;
using TAuth.ResourceAPI.Auth.Policies;

namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(PolicyNames.WorkerOnly, AuthenticationSchemes = OpenIdConnectConstants.AuthSchemes.Introspection)]
    public class BackgroundController : ControllerBase
    {

        [HttpGet]
        public object Get()
        {
            return new
            {
                Status = "Ok",
                Time = DateTimeOffset.UtcNow
            };
        }
    }
}
