using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Cache.Contracts;

namespace MyTrades.Cache;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            //options.InstanceName = "SampleApp_"; // Optional prefix for cache keys
        });

        services.AddScoped<IItemService, ItemService>();

        return services;
    }
}