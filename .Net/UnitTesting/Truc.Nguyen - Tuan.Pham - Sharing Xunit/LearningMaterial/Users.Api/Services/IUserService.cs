using Users.Api.Models;

namespace Users.Api.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User?> GetByIdAsync(Guid id);

    Task<bool> CreateAsync(User user);

    Task<bool> DeleteByIdAsync(Guid id);
}
