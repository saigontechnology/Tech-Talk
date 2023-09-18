using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Api.Commons;

namespace ProductCatalog.Api.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(string userName, string role, CancellationToken cancellationToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.SecretKey]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration[Constants.Issuer],
            audience: _configuration[Constants.Audience],
            claims: new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Actor, userName),
                new Claim(ClaimTypes.Role, role)
            },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        await Task.CompletedTask;

        return jwtSecurityTokenHandler.WriteToken(token);
    }
}