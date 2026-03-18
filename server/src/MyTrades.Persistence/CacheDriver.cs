using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MyTrades.Domain;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public class CacheDriver<TEntity> : ICacheRepository<TEntity>
    where TEntity : IEntity
{
    private readonly IDistributedCache _cache;

    private readonly ILogger<CacheDriver<TEntity>> _logger;

    public CacheDriver(IDistributedCache cache, ILogger<CacheDriver<TEntity>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TEntity> GetItem(string key, CancellationToken token = default)
    {
        string cacheKey = $"Item_{key}";

        var bytes = await _cache.GetAsync(cacheKey, token);
        
        if (bytes != null)
        {
            _logger.LogDebug("✅ Item retrieved from cache!");
            return MessagePackSerializer.Deserialize<TEntity>(bytes);
        }

        throw new KeyNotFoundException($"Item with key {key} not found in cache!");
    }

    public Task SetItem(string key, TEntity item, CancellationToken token = default)
    {
        var bytes = MessagePackSerializer.Serialize(item, MessagePack.Resolvers.ContractlessStandardResolver.Options);

        return _cache.SetAsync(key, bytes, token: token);
    }
}