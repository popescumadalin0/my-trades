using MyTrades.Domain;

namespace MyTrades.Persistence.Contracts;

public interface IEntityStore<TEntity> where TEntity : IEntity
{
    Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default);
    
    Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default);
}