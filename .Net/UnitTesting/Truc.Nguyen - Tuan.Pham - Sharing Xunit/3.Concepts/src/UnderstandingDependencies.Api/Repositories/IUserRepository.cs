using UnderstandingDependencies.Api.Models;

namespace UnderstandingDependencies.Api.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
}
