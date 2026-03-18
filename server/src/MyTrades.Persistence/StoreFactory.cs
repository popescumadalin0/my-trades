using System;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Domain;
using MyTrades.Persistence.Contracts;
using MyTrades.Persistence.Enums;

namespace MyTrades.Persistence;

public class StoreFactory
{
    private readonly IServiceProvider _provider;

    public StoreFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IStore<TEntity> Create<TEntity>(StorageType type)
        where TEntity : IEntity
    {
        return type switch
        {
            StorageType.Postgres => _provider.GetRequiredService<PostgresStore<TEntity>>(),
            StorageType.Redis => _provider.GetRequiredService<RedisStore<TEntity>>(),
            _ => throw new NotSupportedException()
        };
    }
}