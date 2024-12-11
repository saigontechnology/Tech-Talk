using PlanningBook.Identity.Infrastructure.Entities;

namespace PlanningBook.Identity.Application.Providers.Interfaces
{
    public interface ITokenProvider
    {
        string GenerateToken(Account account);
        string GenerateRefreshToken();
    }
}
