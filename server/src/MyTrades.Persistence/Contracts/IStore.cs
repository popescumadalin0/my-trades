using MyTrades.Domain;

namespace MyTrades.Persistence.Contracts;

public interface IStore<TEntity> where TEntity : IEntity
{
    Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);
    
    Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default);
}