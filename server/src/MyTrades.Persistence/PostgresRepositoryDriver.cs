using System.Data;
using Dapper;
using MyTrades.Persistence.Contracts;
using Npgsql;

namespace MyTrades.Persistence;

public class PostgresRepositoryDriver<TEntity> : IRepositoryDriver<TEntity>
{
    private readonly IDbConnection _connection;
    private readonly string _tableName;

    public PostgresRepositoryDriver(NpgsqlConnection connection)
    {
        _connection = connection;
        _tableName = typeof(TEntity).Name.ToLower();
    }

    public Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";

        return _connection.QuerySingleOrDefaultAsync<TEntity>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }

    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName}";

        var result = _connection.QueryAsync<TEntity>(
            new CommandDefinition(sql, cancellationToken: cancellationToken)
        );

        return result;
    }

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties();

        var columns = string.Join(",", properties.Select(p => p.Name.ToLower()));
        var values = string.Join(",", properties.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";

        return _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != "Id");

        var setClause = string.Join(",", properties.Select(p => $"{p.Name.ToLower()} = @{p.Name}"));

        var sql = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";

        return _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_tableName} WHERE id = @Id";

        return _connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }
}