using System.Threading;
using System.Threading.Tasks;
using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class PostgresStore<TEntity> : IStore<TEntity>
    where TEntity : IEntity
{
    private readonly IRepositoryDriver<TEntity> _repositoryDriver;

    public PostgresStore(IRepositoryDriver<TEntity> repositoryDriver)
    {
        _repositoryDriver = repositoryDriver;
    }

    public Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return _repositoryDriver.GetByIdAsync(id, cancellationToken);
    }

    public async Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await GetAsync(entity.Id, cancellationToken);

        if (existingEntity != null)
        {
            await _repositoryDriver.UpdateAsync(entity, cancellationToken);
        }
        else
        {
            await _repositoryDriver.InsertAsync(entity, cancellationToken);
        }
    }
}