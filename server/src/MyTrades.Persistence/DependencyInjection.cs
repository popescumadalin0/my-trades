using Dapper;
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
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            //options.InstanceName = "SampleApp_"; // Optional prefix for cache keys
        });

        services.AddScoped<DapperDbContext>(_ => new DapperDbContext(config.GetConnectionString("DefaultConnection")!));

        services.AddScoped<IRepositoryDriver<Candle>, PostgresRepositoryDriver<Candle>>();
        services.AddScoped<IStore<Candle>, PostgresStore<Candle>>();
        services.AddScoped<IRepositoryDriver<Symbol>, PostgresRepositoryDriver<Symbol>>();
        services.AddScoped<IStore<Symbol>, PostgresStore<Symbol>>();
        services.AddScoped<IRepositoryDriver<DeadLetterEntry>, PostgresRepositoryDriver<DeadLetterEntry>>();
        services.AddScoped<IStore<DeadLetterEntry>, PostgresStore<DeadLetterEntry>>();

        services.AddScoped<ICacheRepository<Candle>, CacheDriver<Candle>>();
        services.AddScoped<IStore<Candle>, RedisStore<Candle>>();

        services.AddScoped<StoreFactory>();

        return services;
    }
}