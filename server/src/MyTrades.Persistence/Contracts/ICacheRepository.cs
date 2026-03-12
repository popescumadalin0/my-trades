namespace MyTrades.Persistence.Contracts;

public interface ICacheRepository<TEntity>
{
    Task SetItem(string key, TEntity item, CancellationToken cancellationToken = default);
    Task<TEntity> GetItem(string key, CancellationToken cancellationToken = default);
}