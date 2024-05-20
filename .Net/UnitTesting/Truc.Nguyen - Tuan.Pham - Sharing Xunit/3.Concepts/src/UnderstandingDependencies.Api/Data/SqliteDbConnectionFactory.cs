using System.Data;
using Microsoft.Data.Sqlite;
using UnderstandingDependencies.Api.Options;

namespace UnderstandingDependencies.Api.Data;

public class SqliteDbConnectionFactory
{
    private readonly DbConnectionOptions _connectionOptions;

    public SqliteDbConnectionFactory()
    {
        _connectionOptions = new DbConnectionOptions
        {
            ConnectionString = "Data Source=./database.db"
        };
    }

    public async Task<IDbConnection> CreateDbConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionOptions.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
