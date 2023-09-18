using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Entities;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repository;

public class UserRepository : IUserRepository
{
    private readonly ProductCatalogDbContext _db;
    private readonly IMapper _mapper;
    
    public UserRepository(ProductCatalogDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UserInformation> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        await _db.Users.AddAsync(_mapper.Map<User>(registerRequest), cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserInformation>(registerRequest);
    }
    
    public async Task<bool> CheckUserExistsAsync(string userName, CancellationToken cancellationToken = default)
        => await _db.Users.AnyAsync(a => a.UserName == userName, cancellationToken);
    
    public async Task<User> GetUserByUserName(string userName, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => 
            u.IsActive && u.UserName == userName, cancellationToken);

        return user;
    }
}