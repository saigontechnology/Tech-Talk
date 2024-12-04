using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VinhNgo.Sample.gRPC.Entities;

namespace VinhNgo.Sample.gRPC.Services;

public class UserService(MainDBContext _dbContext) : User.UserBase
{
    public override async Task<UserCreateResponse> Create(UserCreateRequest request, ServerCallContext context)
    {
        var user = await CreateUser(request);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new UserCreateResponse
        {
            Id = user.Id.ToString()
        };
    }

    public override async Task<UserCreateRequest> GetById(UserCreateResponse request, ServerCallContext context)
    {
        return await GetByIdAsync(request.Id);
    }

    public override async Task<UserItems> Gets(UserQuery request, ServerCallContext context)
    {
        var users = await GetListAsync(request);

        var rs = new UserItems();
        
        rs.Items.AddRange(users);

        return rs;
    }

    public override async Task<UserCreateRequest> Get(UserCreateResponse request, ServerCallContext context)
    {
        return await GetByIdAsync(request.Id);
    }

    public override async Task StreamingUsers(UserQuery request, IServerStreamWriter<UserFullName> responseStream, ServerCallContext context)
    {
        var users = await GetListAsync(request);
        
        foreach (var user in users)
        {
            await responseStream.WriteAsync(new UserFullName { Name = string.Join(user.FirstName, user.LastName, " ") });
            
            await Task.Delay(1000);
        }
    }

    public override async Task<CountCreatedUser> BulkCreate(IAsyncStreamReader<UserCreateRequest> requestStream, ServerCallContext context)
    {
        var count = 0;
        await foreach (var userRequest in requestStream.ReadAllAsync(context.CancellationToken)) {
            
            var user = await CreateUser(userRequest, context.CancellationToken);

            count++;
        }

        await _dbContext.SaveChangesAsync();

        return new CountCreatedUser
        {
            Count = count
        };
    }

    public override async Task BulkCreate2(IAsyncStreamReader<UserCreateRequest> requestStream, IServerStreamWriter<CountCreatedUser> responseStream, ServerCallContext context)
    {
        var count = 0;
        await foreach (var userRequest in requestStream.ReadAllAsync()) {
            
            var user = await CreateUser(userRequest);
            
            await _dbContext.SaveChangesAsync();

            count++;
            
            await responseStream.WriteAsync(new CountCreatedUser { Count = count });
        }
    }

    public override async Task<TokenModel> Login(UserCreateResponse request, ServerCallContext context)
    {
        return new TokenModel
        {
            Token = await Login(request.Id)
        };
    }

    [Authorize]
    public override async Task<UserCreateRequest> GetByIdWithAuthorize(UserCreateResponse request, ServerCallContext context)
    {
        return await GetByIdAsync(request.Id);
    }

    private async Task<UserEntity> CreateUser(UserCreateRequest request, CancellationToken cancellationToken = default)
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };
        
        await _dbContext.AddAsync(user, cancellationToken);

        return user;
    }
    
    private async Task<List<UserCreateRequest>> GetListAsync(UserQuery request)
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(x => x.FirstName.Contains(request.Keyword));
        }

        return await query
            .Select(x => new UserCreateRequest
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                Password = x.Password,
                Email = x.FirstName,
            }).Take(8)
            .ToListAsync();
    }

    private async Task<UserCreateRequest> GetByIdAsync(string id)
    {
        return await _dbContext.Users.Where(x => x.Id.ToString() == id)
            .Select(x => new UserCreateRequest
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                Password = x.Password,
                Email = x.FirstName,
            }).FirstOrDefaultAsync();
    }

    private async Task<string> Login(string id)
    {
        var user = await _dbContext.Users.Where(x => x.Id.ToString() == id)
            .Select(x => new 
            {
                x.Id,
               x.Email,
            }).FirstOrDefaultAsync();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        var (_, token) = GetSecurityToken(claims);

        return token;
    }
    
    private (SecurityToken sToken, string Token) GetSecurityToken(List<Claim> authClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = GetTokenDescriptor(authClaims);
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        
        return (securityToken, token);
    }
    
    private SecurityTokenDescriptor GetTokenDescriptor(List<Claim> authClaims)
    {
        var authSigningKey =
            new SymmetricSecurityKey(
                Base64UrlEncoder.DecodeBytes("TMZJC9SZZXj2ytcs5m2b37nvh3trrhbz9ib7ncwitcv"));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenDescriptor;
    }
}