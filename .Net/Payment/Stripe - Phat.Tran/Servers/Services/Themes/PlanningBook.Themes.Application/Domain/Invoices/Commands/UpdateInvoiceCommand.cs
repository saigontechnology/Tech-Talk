using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;
using PlanningBook.Themes.Infrastructure.Entities.Enums;

namespace PlanningBook.Themes.Application.Domain.Invoices.Commands
{
    public sealed class UpdateInvoiceCommand : ICommand<CommandResult<Guid>>
    {
        public Guid InvoiceId { get; set; }
        public bool IsSuccess { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class UpdateInvoiceCommandHandler(
        IEFClassRepository<PBThemeDbContext, Invoice, Guid> _invoiceRepository) : ICommandHandler<UpdateInvoiceCommand, CommandResult<Guid>>
    {
        public async Task<CommandResult<Guid>> HandleAsync(UpdateInvoiceCommand command, CancellationToken cancellationToken = default)
        {
            var invoiceExisted = await _invoiceRepository.GetByIdAsync(command.InvoiceId, cancellationToken);
            if (invoiceExisted == null)
                return CommandResult<Guid>.Failure("Invoice not existed");

            if (command.IsSuccess)
                invoiceExisted.PaymentStatus = PaymentStatus.Success;
            else
                invoiceExisted.PaymentStatus = PaymentStatus.Cancel;

            await _invoiceRepository.UpdateAsync(invoiceExisted, cancellationToken);
            await _invoiceRepository.SaveChangeAsync(cancellationToken);

            return CommandResult<Guid>.Success(invoiceExisted.Id);
        }
    }
}
