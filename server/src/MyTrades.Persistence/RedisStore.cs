using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class RedisStore<TEntity> : IStore<TEntity>
    where TEntity : IEntity
{
    private readonly ICacheRepository<TEntity> _cacheRepository;

    public RedisStore(ICacheRepository<TEntity> cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return _cacheRepository.GetItem(id.ToString(), cancellationToken);
    }

    public Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var cacheKey = entity.Id.ToString();

        return _cacheRepository.SetItem(cacheKey, entity, cancellationToken);
    }
}