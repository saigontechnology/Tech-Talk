using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Infrastructure;
using PlanningBook.Themes.Infrastructure.Entities;

namespace PlanningBook.Themes.Application.Domain.StripeCustomers.Queries
{
    public sealed class GetUserStripeCustomerQuery : IQuery<QueryResult<StripeCustomer>>
    {
        public Guid UserId { get; set; }
        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }

    public class GetUserStripeCustomerQueryHandler(
        IEFRepository<PBThemeDbContext, StripeCustomer, Guid> _stripeCustomerRepository) : IQueryHandler<GetUserStripeCustomerQuery, QueryResult<StripeCustomer>>
    {
        public async Task<QueryResult<StripeCustomer>> HandleAsync(GetUserStripeCustomerQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _stripeCustomerRepository.GetFirstAsync(x => x.UserId == query.UserId, cancellationToken);

            if (result == null)
            {
                QueryResult<StripeCustomer>.Failure("User still haven't Stripe Customer information");
            }

            return QueryResult<StripeCustomer>.Success(result);
        }
    }
}
