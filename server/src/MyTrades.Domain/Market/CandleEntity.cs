namespace MyTrades.Domain.Market;

public class CandleEntity
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public DateTime Time { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
    public decimal OpenInterest { get; set; }
    public decimal HighestPrice { get; set; }
    public decimal LowestPrice { get; set; }
    public decimal ClosePrice { get; set; }
}

//todo: create IStores, IRepository si IStateStore
public interface IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(string id);
    Task<List<TEntity>> GetAllAsync();
    Task InsertAsync(TEntity entity);
    
    Task UpdateAsync(TEntity entity);
    
    Task DeleteAsync(TEntity entity);
}

public class Repository<TEntity> : IRepository<TEntity>
{
    public Task<TEntity> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}


//candles and others that are saved in the cache
public interface IStore<TEntity>
{
    Task<TEntity> GetAsync(string id);
    
    Task StoreAsync(TEntity entity);
}

public class PostgreSqlStore<TEntity> : IStore<TEntity>
{
    //call repository
    public Task<TEntity> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task StoreAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}

public class RedisStore<TEntity> : IStore<TEntity>
{
    //call cache shit
    public Task<TEntity> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task StoreAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}

public interface IStateStore<TEntity>
{
    Task<List<TEntity>> GetAsync(string id);
    
    Task SaveAsync(TEntity entity);
}

public class MemoryStateStore<TEntity> : IStateStore<TEntity>
{
    public Task<List<TEntity>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}

public class RedisStateStore<TEntity> : IStateStore<TEntity>
{
    public Task<List<TEntity>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}

public class PostgreSqlStateStore<TEntity> : IStateStore<TEntity>
{
    public Task<List<TEntity>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}