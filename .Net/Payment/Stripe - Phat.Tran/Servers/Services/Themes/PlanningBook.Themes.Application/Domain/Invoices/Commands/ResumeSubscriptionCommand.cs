using PlanningBook.Domain.Interfaces;
using PlanningBook.Domain;
using PlanningBook.Themes.Application.Services;

namespace PlanningBook.Themes.Application.Domain.Invoices.Commands
{
    public sealed class ResumeSubscriptionCommand : ICommand<CommandResult<bool>>
    {
        public string SubscriptionId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class ResumeSubscriptionCommandHandler(StripePaymentService _stripeService) : ICommandHandler<ResumeSubscriptionCommand, CommandResult<bool>>
    {
        public async Task<CommandResult<bool>> HandleAsync(ResumeSubscriptionCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _stripeService.ResumeSubsriptionAsyn(command.SubscriptionId);

                return CommandResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return CommandResult<bool>.Failure($"{ex.Message.ToString()}");
            }
        }
    }
}
