using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MyTrades.Cache.Contracts;

namespace MyTrades.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    private ILogger<CacheService> _logger;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T> GetItem<T>(string key)
    {
        string cacheKey = $"Item_{key}";

        var bytes = await _cache.GetAsync(cacheKey);
        if (bytes != null)
        {
            _logger.LogDebug("✅ Item retrieved from cache!");
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        throw new KeyNotFoundException($"Item with key {key} not found in cache!");
    }

    public async Task SetItem<T>(string key, T item)
    {
        var bytes = MessagePackSerializer.Serialize(item, MessagePack.Resolvers.ContractlessStandardResolver.Options);

        await _cache.SetAsync(key, bytes);
    }
}