using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Domain.Invoices.Queries.Models;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.Application.Domain.Invoices.Queries
{
    public sealed class GetUserInvoicesQuery : IQuery<QueryResult<List<UserInvoiceModel>>>
    {
        public Guid UserId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class GetUserInvoicesQueryHandler(
        IEFRepository<PBThemeDbContext, Invoice, Guid> _invoiceRepository) : IQueryHandler<GetUserInvoicesQuery, QueryResult<List<UserInvoiceModel>>>
    {
        public async Task<QueryResult<List<UserInvoiceModel>>> HandleAsync(GetUserInvoicesQuery query, CancellationToken cancellationToken = default)
        {
            var invoices = await _invoiceRepository.GetAsync(x => x.UserId == query.UserId);
            var results = invoices.Select(x => new UserInvoiceModel()
            {
                InvoiceId = x.Id,
                Amount = x.ActualyTotalAmout,
                Status = x.PaymentStatus.ToString()
            }).ToList();

            return QueryResult<List<UserInvoiceModel>>.Success(results);
        }
    }
}
