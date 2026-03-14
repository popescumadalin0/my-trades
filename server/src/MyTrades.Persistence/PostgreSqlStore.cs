using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class PostgreSqlStore<TEntity> : IEntityStore<TEntity>
    where TEntity : IEntity
{
    private readonly IDbRepository<TEntity> _repository;

    public PostgreSqlStore(IDbRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await GetAsync(entity.Id, cancellationToken);

        if (existingEntity != null)
        {
            await _repository.UpdateAsync(entity, cancellationToken);
        }
        else
        {
            await _repository.InsertAsync(entity, cancellationToken);
        }
    }
}