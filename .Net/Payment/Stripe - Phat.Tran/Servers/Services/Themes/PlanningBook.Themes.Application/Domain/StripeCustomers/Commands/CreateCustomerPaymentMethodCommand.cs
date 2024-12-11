using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Services;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.Application.Domain.StripeCustomers.Commands
{
    public sealed class CreateCustomerPaymentMethodCommand : ICommand<CommandResult<string>>
    {
        public Guid UserId { get; set; }
        public string? ByPassCode { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class CreateCustomerPaymentMethodCommandHandler(
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _stripeCustomerRepository,
        StripePaymentService _stripeService) : ICommandHandler<CreateCustomerPaymentMethodCommand, CommandResult<string>>
    {
        public async Task<CommandResult<string>> HandleAsync(CreateCustomerPaymentMethodCommand command, CancellationToken cancellationToken = default)
        {
            var customerExisted = await _stripeCustomerRepository.GetFirstAsync(x => x.UserId == command.UserId, cancellationToken);
            if (customerExisted == null)
            {
                return CommandResult<string>.Failure("Customer is not existed");
            }

            if (string.IsNullOrWhiteSpace(customerExisted.StripeCustomerId))
            {
                var customerStripe = await _stripeService.CreateCustomerAsync(command.UserId);
                if(customerStripe == null)
                {
                    return CommandResult<string>.Failure("Something went wrong!");
                }

                customerExisted.StripeCustomerId = customerStripe;
            }

            var customerPayment = await _stripeService.CreatePaymentMethodAsync(command.ByPassCode);
            if (customerPayment == null)
                return CommandResult<string>.Failure("Create Failed");

            await _stripeCustomerRepository.UpdateAsync(customerExisted, cancellationToken);
            await _stripeCustomerRepository.SaveChangeAsync(cancellationToken);

            return CommandResult<string>.Success(customerPayment);
        }
    }
}
