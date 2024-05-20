using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;

namespace UnderstandingDependencies.Api.Tests.Unit;

public class FakeUserRepository : IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(Enumerable.Empty<User>());
    }
}

public class FakeUserRepositoryWithUsers : IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(new []
        {
            new User
            {
                Id = Guid.NewGuid(),
                FullName = "Tuan Pham"
            }
        });
    }
}

