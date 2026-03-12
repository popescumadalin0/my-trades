using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class RedisStore<TEntity> : IEntityStore<TEntity>
    where TEntity : IEntity
{
    private readonly ICacheRepository<TEntity> _cacheRepository;

    public RedisStore(ICacheRepository<TEntity> cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _cacheRepository.GetItem(id, cancellationToken);

        return entity;
    }

    public async Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var cacheKey = entity.Id;

        await _cacheRepository.SetItem(cacheKey, entity, cancellationToken);
    }
}