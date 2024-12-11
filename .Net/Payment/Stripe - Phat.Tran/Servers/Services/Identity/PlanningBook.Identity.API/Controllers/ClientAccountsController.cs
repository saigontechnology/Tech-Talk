using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Extensions;
using PlanningBook.Identity.Application.Accounts.Commands;
using PlanningBook.Identity.Application.ClientAccounts.Commands;
using PlanningBook.Identity.Application.ClientAccounts.Commands.CommandResults;

namespace PlanningBook.Identity.API.Controllers
{
    [ApiController]
    [Route("identity")]
    public class ClientAccountsController(
        IQueryExecutor _queryExecutor,
        ICommandExecutor _commandExecutor,
        HttpClient _httpClient
        ) : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<CommandResult<Guid>>> SignUp([FromBody] SignUpClientAccountCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<CommandResult<SignInClientAccountCommandResult>>> SignIn([FromBody] SignInClientAccountCommand command)
        {
            
            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("SignOut")]
        public async Task<ActionResult<CommandResult<bool>>> SignOut()
        {
            //var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var command = new SignOutClientAccountCommand()
            {
                AccountId = User.GetCurrentAccountId(),
                Token = Request.GetCurrentJwtToken()
            };
            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
