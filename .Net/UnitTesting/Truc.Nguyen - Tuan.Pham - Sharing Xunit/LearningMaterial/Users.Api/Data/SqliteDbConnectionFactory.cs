using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Users.Api.Options;

namespace Users.Api.Data;

public class SqliteDbConnectionFactory : ISqliteDbConnectionFactory
{
    private readonly DbConnectionOptions _connectionOptions;

    public SqliteDbConnectionFactory(IOptions<DbConnectionOptions> connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    public async Task<IDbConnection> CreateDbConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionOptions.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
