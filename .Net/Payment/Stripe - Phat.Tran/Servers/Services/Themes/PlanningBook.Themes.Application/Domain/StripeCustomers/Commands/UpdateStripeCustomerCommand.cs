using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Services;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.Application.Domain.StripeCustomers.Commands
{
    public sealed class UpdateStripeCustomerCommand : ICommand<CommandResult<StripeCustomer>>
    {
        public Guid UserId { get; set; }
        public string PaymentMethodId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class UpdateStripeCustomerCommandHandler(
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _stripeCustomerRepository,
        StripePaymentService _stripePaymentSerice) : ICommandHandler<UpdateStripeCustomerCommand, CommandResult<StripeCustomer>>
    {
        public async Task<CommandResult<StripeCustomer>> HandleAsync(UpdateStripeCustomerCommand command, CancellationToken cancellationToken = default)
        {
            var customer = await _stripeCustomerRepository.GetFirstAsync(x => x.UserId == command.UserId, cancellationToken);
            if (customer == null)
            {
                return CommandResult<StripeCustomer>.Failure("Customer not exited");
            }

            if (customer.StripePaymentMethodId == command.PaymentMethodId)
            {
                await _stripePaymentSerice.DetachPaymentMethodAsync(command.PaymentMethodId);
                customer.StripePaymentMethodId = null;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(customer.StripePaymentMethodId))
                    await _stripePaymentSerice.DetachPaymentMethodAsync(customer.StripePaymentMethodId);
                await _stripePaymentSerice.AttachPaymentMethodAsync(customer.StripeCustomerId, command.PaymentMethodId);
                customer.StripePaymentMethodId = command.PaymentMethodId;
            }

            await _stripeCustomerRepository.UpdateAsync(customer, cancellationToken);
            await _stripeCustomerRepository.SaveChangeAsync(cancellationToken);

            return CommandResult<StripeCustomer>.Success(customer);
        }
    }
}
