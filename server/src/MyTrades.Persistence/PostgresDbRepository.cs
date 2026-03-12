using Dapper;
using Npgsql;
using System.Data;
using MyTrades.Persistence.Contracts;

public class PostgresDbRepository<TEntity> : IDbRepository<TEntity>
{
    private readonly IDbConnection _connection;
    private readonly string _tableName;

    public PostgresDbRepository(NpgsqlConnection connection)
    {
        _connection = connection;
        _tableName = typeof(TEntity).Name.ToLower();
    }

    public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";

        return await _connection.QuerySingleOrDefaultAsync<TEntity>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName}";

        var result = await _connection.QueryAsync<TEntity>(
            new CommandDefinition(sql, cancellationToken: cancellationToken)
        );

        return result.ToList();
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties();

        var columns = string.Join(",", properties.Select(p => p.Name.ToLower()));
        var values = string.Join(",", properties.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";

        await _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != "Id");

        var setClause = string.Join(",", properties.Select(p => $"{p.Name.ToLower()} = @{p.Name}"));

        var sql = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";

        await _connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_tableName} WHERE id = @Id";

        await _connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }
}