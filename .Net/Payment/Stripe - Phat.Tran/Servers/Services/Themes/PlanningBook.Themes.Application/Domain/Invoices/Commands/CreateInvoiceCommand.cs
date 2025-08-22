using PlanningBook.Domain.Interfaces;
using PlanningBook.Domain;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;
using PlanningBook.Themes.Application.Services;
using PlanningBook.Themes.Infrastructure.Entities.Enums;
using PlanningBook.Themes.Application.Domain.Invoices.Commands.Models;
using Stripe.Checkout;

namespace PlanningBook.Themes.Application.Domain.Invoices.Commands
{
    public sealed class CreateInvoiceCommand : ICommand<CommandResult<CreateInvoiceCommandResult>>
    {
        public string OriginUrl { get; set; }
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public decimal? Price { get; set; }
        public bool IsUseStripePrice { get; set; }
        public bool IsAutoSavePayment { get; set; } = false;
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class CreateInvoiceCommandHandler(
        IEFRepository<PBThemeDbContext, Invoice, Guid> _invoiceRepository,
        IEFRepository<PBThemeDbContext, Product, Guid> _productRepository,
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _customerStripeRepository,
        StripePaymentService _stripePaymentService) : ICommandHandler<CreateInvoiceCommand, CommandResult<CreateInvoiceCommandResult>>
    {
        public async Task<CommandResult<CreateInvoiceCommandResult>> HandleAsync(CreateInvoiceCommand command, CancellationToken cancellationToken = default)
        {
            var userExisted = await _customerStripeRepository.GetFirstAsync(x => x.UserId == command.UserId, cancellationToken);
            if (userExisted == null)
                return CommandResult<CreateInvoiceCommandResult>.Failure("User Not Existed");

            var productExited = await _productRepository.GetFirstAsync(x => x.Id == command.ProductId, cancellationToken);
            if (productExited == null)
                return CommandResult<CreateInvoiceCommandResult>.Failure("Product Not Existed");

            var invoice = new Invoice()
            {
                UserId = userExisted.UserId,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = productExited.Price,
                ActualyTotalAmout = productExited.Price,
                ProductId = productExited.Id,
                Notes = $"{DateTime.Now.ToString()} - {userExisted.StripeCustomerId}"
            };
            await _invoiceRepository.AddAsync(invoice, cancellationToken);
            await _invoiceRepository.SaveChangeAsync(cancellationToken);

            var actualyProductPrice = command.Price ?? productExited.Price;
            var customerId = command.IsAutoSavePayment ? userExisted.StripeCustomerId : null;
            Session session = null;
            if (command.IsUseStripePrice)
                session = await _stripePaymentService.CheckoutSessionAsync(command.OriginUrl, userExisted.UserId, invoice.Id, productExited.ProductType, 0, productExited.StripePriceId, null, customerId);
            else
                session = await _stripePaymentService.CheckoutSessionAsync(command.OriginUrl, userExisted.UserId, invoice.Id, productExited.ProductType, actualyProductPrice, null, null, customerId);

            if(session != null)
            {
                var result = new CreateInvoiceCommandResult()
                {
                    InvoiceId = invoice.Id,
                    StripeSessionId = session.Id,
                    Mode = session.Mode,
                    ActuallyAmountTotal = session.AmountSubtotal,
                    AmountTotal = session.AmountTotal,
                    Currency = session.Currency,
                    Url = session.Url,
                    SubscriptionId = session.SubscriptionId
                };
                return CommandResult<CreateInvoiceCommandResult>.Success(result);
            }

            return CommandResult<CreateInvoiceCommandResult>.Failure("End Wrong!");
        }
    }
}
