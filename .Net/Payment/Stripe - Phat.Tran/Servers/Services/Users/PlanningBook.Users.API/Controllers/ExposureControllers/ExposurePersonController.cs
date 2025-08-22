using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Users.Application.Persons.Commands;

namespace PlanningBook.Users.API.Controllers.ExposureControllers
{
    // TODO: Add secret key to call Server-to-Server
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExposurePersonController : ControllerBase
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;

        public ExposurePersonController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<CommandResult<Guid>>> CreatePersonBySystem([FromBody] CreatePersonCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
