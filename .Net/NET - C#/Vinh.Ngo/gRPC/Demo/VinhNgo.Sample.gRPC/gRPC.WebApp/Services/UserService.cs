using VinhNgo.Sample.gRPC;

namespace gRPC.WebApp.Services;

public class UserService(User.UserClient _client)
{
    public async Task<List<UserCreateRequest>> Gets()
    {
        var users = await _client.GetsAsync(new UserQuery
        {
            Keyword = ""
        });
    
        return users.Items.ToList();
    }
}