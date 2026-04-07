using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class PostgresRepositoryDriver<TEntity> : IRepositoryDriver<TEntity>
{
    private readonly DapperDbContext _dbContext;
    private readonly string _tableName;

    public PostgresRepositoryDriver(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
        _tableName = typeof(TEntity).Name.ToLower();
    }

    public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
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
        var properties = typeof(TEntity).GetProperties();

        var columns = string.Join(",", properties.Select(p => p.Name.ToLower()));
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
            .Where(p => p.Name != "Id");

        var setClause = string.Join(",", properties.Select(p => $"{p.Name.ToLower()} = @{p.Name}"));

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