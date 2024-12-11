using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Extensions;
using PlanningBook.Themes.Application.Domain.Invoices.Commands;
using PlanningBook.Themes.Application.Domain.StripeCustomers.Commands;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class StripeCustomersController(
        IQueryExecutor _queryExecutor,
        ICommandExecutor _commandExecutor) : ControllerBase
    {
        [HttpPost("create-stripe-customer")]
        public async Task<ActionResult<CommandResult<string>>> CreateStripeCustomerAsync()
        {
            var currentUserId = User.GetCurrentAccountId() ?? Guid.Empty;
            var command = new CreateStripeCustomerCommand()
            {
                UserId = currentUserId
            };

            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("create-stripe-payment-method")]
        public async Task<ActionResult<CommandResult<string>>> CreateStripePaymentMethodAsync([FromBody] CreateCustomerPaymentMethodCommand command)
        {
            var currentUserId = User.GetCurrentAccountId() ?? Guid.Empty;
            command.UserId = currentUserId;

            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("cancel-subscription")]
        public async Task<ActionResult<CommandResult<bool>>> CancelSubscriptionAsync([FromBody] CancelSubscriptionCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut("update-customer")]
        public async Task<ActionResult<CommandResult<StripeCustomer>>> UpdateCustomerAsync([FromBody] UpdateStripeCustomerCommand command)
        {
            var currentUserId = User.GetCurrentAccountId() ?? Guid.Empty;
            command.UserId = currentUserId;

            var result = await _commandExecutor.ExecuteAsync(command);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
