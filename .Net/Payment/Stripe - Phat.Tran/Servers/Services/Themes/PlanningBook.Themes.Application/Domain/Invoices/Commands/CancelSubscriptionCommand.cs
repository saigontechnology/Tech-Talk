using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Themes.Application.Services;

namespace PlanningBook.Themes.Application.Domain.Invoices.Commands
{
    public sealed class CancelSubscriptionCommand : ICommand<CommandResult<bool>>
    {
        public string SubscriptionId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class CancelSubscriptionCommandHandler(StripePaymentService _stripeService) : ICommandHandler<CancelSubscriptionCommand, CommandResult<bool>>
    {
        public async Task<CommandResult<bool>> HandleAsync(CancelSubscriptionCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _stripeService.CancelSubscriptionsAsyn(command.SubscriptionId);

                return CommandResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return CommandResult<bool>.Failure($"{ex.Message.ToString()}");
            }
        }
    }
}
