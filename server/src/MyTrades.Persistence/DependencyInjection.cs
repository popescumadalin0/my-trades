using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Domain.Market;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MigrationRunner>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            //options.InstanceName = "SampleApp_"; // Optional prefix for cache keys
        });

        services.AddScoped<IDbRepository<Candle>, PostgresDbRepository<Candle>>();
        services.AddScoped<ICacheRepository<Candle>, CacheRepository<Candle>>();
        services.AddScoped<IEntityStore<Candle>, PostgreSqlStore<Candle>>();
        services.AddScoped<IEntityStore<Candle>, RedisStore<Candle>>();
        
        services.AddScoped<IDbRepository<Symbol>, PostgresDbRepository<Symbol>>();
        services.AddScoped<ICacheRepository<Symbol>, CacheRepository<Symbol>>();
        services.AddScoped<IEntityStore<Symbol>, PostgreSqlStore<Symbol>>();
        services.AddScoped<IEntityStore<Symbol>, RedisStore<Symbol>>();

        return services;
    }
}