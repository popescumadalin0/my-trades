using MyTrades.Domain;

namespace MyTrades.Persistence.Contracts;

public interface IRepositoryDriver<TEntity>
    where TEntity : IEntity
{
    Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}