using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;

        public HealthCheckController(
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [AllowAnonymous]
        [HttpGet("NonAuth")]
        public ActionResult NoAuth()
        {
            return Ok("Still Live");
        }

        [HttpGet("HasAuth")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult HasAuth()
        {
            return Ok("Still Live");
        }
    }
}
