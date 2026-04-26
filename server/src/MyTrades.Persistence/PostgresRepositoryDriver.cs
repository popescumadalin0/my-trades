using System.Reflection;
using Dapper;
using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class PostgresRepositoryDriver<TEntity> : IRepositoryDriver<TEntity>
    where TEntity : IEntity
{
    private readonly DapperDbContext _dbContext;
    private readonly string _tableName;

    public PostgresRepositoryDriver(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
        _tableName = typeof(TEntity).GetCustomAttribute<TableNameAttribute>()?.Name
                     ?? typeof(TEntity).Name.ToSnakeCase();
    }

    public async Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";

        using var connection = _dbContext.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<TEntity>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_tableName}";

        using var connection = _dbContext.GetConnection();
        var result = await connection.QueryAsync<TEntity>(
            new CommandDefinition(sql, cancellationToken: cancellationToken)
        );

        return result;
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != nameof(IEntity.Id));

        var columns = string.Join(",", properties.Select(p => p.Name.ToSnakeCase()));
        var values = string.Join(",", properties.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";

        using var connection = _dbContext.GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != nameof(IEntity.Id));

        var setClause = string.Join(",", properties.Select(p => $"{p.Name.ToSnakeCase()} = @{p.Name}"));

        var sql = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";

        using var connection = _dbContext.GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sql, entity, cancellationToken: cancellationToken)
        );
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_tableName} WHERE id = @Id";

        using var connection = _dbContext.GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
        );
    }
}