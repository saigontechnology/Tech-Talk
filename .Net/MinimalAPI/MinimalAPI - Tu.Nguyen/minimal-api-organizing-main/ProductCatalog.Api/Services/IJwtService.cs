namespace ProductCatalog.Api.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(string userName, string role, CancellationToken cancellationToken);
}