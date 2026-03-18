using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyTrades.Persistence.Contracts;

public interface IRepositoryDriver<TEntity>
{
    Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}