using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Services;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.Application.Domain.StripeCustomers.Commands
{
    public sealed class CreateStripeCustomerCommand : ICommand<CommandResult<string>>
    {
        public Guid UserId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class CreateStripeCustomerCommandHandler(
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _stripeCustomerRepository,
        StripePaymentService _stripePaymentService) : ICommandHandler<CreateStripeCustomerCommand, CommandResult<string>>
    {
        public async Task<CommandResult<string>> HandleAsync(CreateStripeCustomerCommand command, CancellationToken cancellationToken = default)
        {
            var stripeCustomerId = await _stripePaymentService.CreateCustomerAsync(command.UserId);

            var stripeCustomer = new StripeCustomer()
            {
                StripeCustomerId = stripeCustomerId,
                UserId = command.UserId,
            };

            await _stripeCustomerRepository.AddAsync(stripeCustomer);
            await _stripeCustomerRepository.SaveChangeAsync();

            return CommandResult<string>.Success(stripeCustomerId);
        }
    }
}
