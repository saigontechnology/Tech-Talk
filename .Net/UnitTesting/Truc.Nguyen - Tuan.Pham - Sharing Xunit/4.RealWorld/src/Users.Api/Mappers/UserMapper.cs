using Users.Api.Contracts;
using Users.Api.Models;

namespace Users.Api.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName
        };
    }
}
