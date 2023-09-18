using ProductCatalog.Api.Data.Entities;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public interface IUserRepository
{
    Task<bool> CheckUserExistsAsync(string userName, CancellationToken cancellationToken = default);
    Task<UserInformation> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<User> GetUserByUserName(string userName, CancellationToken cancellationToken = default);
}