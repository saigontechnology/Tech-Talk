using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthNET.Sharing.WebApp.Services
{
    public interface IIdentityService
    {
        Task<AppUserModel> GetUserAsync(string userId);
        Task<ClaimsPrincipal> GetUserPrincipalAsync(AppUserModel user, string authScheme);
        Task<AppUserModel> AuthenticateAsync(string username, string password);
        Task<AppUserModel> AuthenticateAsync(string apiKey);
        Task<bool> HasUserBeenChangedSince(string userId, DateTimeOffset time);
    }

    public class IdentityService : IIdentityService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public IdentityService(DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public Task<AppUserModel> AuthenticateAsync(string username, string password)
        {
            var user = _dataContext.Users
                .Where(user => user.UserName == username && user.Password == password)
                .ProjectTo<AppUserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        public Task<AppUserModel> AuthenticateAsync(string apiKey)
        {
            var user = _dataContext.Users
                .Where(user => user.ApiKey == apiKey)
                .ProjectTo<AppUserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        public Task<AppUserModel> GetUserAsync(string userId)
        {
            var user = _dataContext.Users
                .Where(user => user.Id == userId)
                .ProjectTo<AppUserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        public Task<ClaimsPrincipal> GetUserPrincipalAsync(AppUserModel user, string authScheme)
        {
            var claimsIdentity = new ClaimsIdentity(authScheme);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
            claimsIdentity.AddClaims(
                user.Roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray());
            claimsIdentity.AddClaims(
                user.FinalPermissions.Select(perm => new Claim(AuthConstants.AppClaimTypes.Permission, perm)).ToArray());
            claimsIdentity.AddClaim(new Claim(AuthConstants.AppClaimTypes.UserName, user.UserName));

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Task.FromResult(claimsPrincipal);
        }

        public async Task<bool> HasUserBeenChangedSince(string userId, DateTimeOffset time)
        {
            var hasChanged = await _dataContext.Users.Where(u => u.Id == userId)
                .AnyAsync(u => u.LastChanged > time);

            return hasChanged;
        }
    }
}
