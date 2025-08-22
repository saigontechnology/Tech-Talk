using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Services;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlanningBook.Themes.Application.Domain.Invoices.Commands
{
    public sealed class CreatePaymentIntentCommand : ICommand<CommandResult<bool>>
    {
        public Guid UserId { get; set; }
        public long Amount { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class CreatePaymentIntentCommandHandler(
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _stripeCustomerRepository,
        StripePaymentService _stripeService) : ICommandHandler<CreatePaymentIntentCommand, CommandResult<bool>>
    {
        public async Task<CommandResult<bool>> HandleAsync(CreatePaymentIntentCommand command, CancellationToken cancellationToken = default)
        {
            var userExsited = await _stripeCustomerRepository.GetFirstAsync(x => x.UserId == command.UserId);
            if (userExsited == null)
                return CommandResult<bool>.Failure("User Not Found!");

            var result = await _stripeService.CreatePaymentIntentAsync(userExsited.StripeCustomerId, userExsited.StripePaymentMethodId, command.Amount);

            return CommandResult<bool>.Success(true);
      
        }
    }
}
