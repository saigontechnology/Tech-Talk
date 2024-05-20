using System.Data;

namespace Users.Api.Data;

public interface ISqliteDbConnectionFactory
{
    Task<IDbConnection> CreateDbConnectionAsync();
}
