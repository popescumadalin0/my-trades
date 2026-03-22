using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.Strategy.EventHandlers;
using MyTrades.Strategy.Profiles;

namespace MyTrades.Strategy;

public static class DependencyInjection
{
    public static IServiceCollection RegisterStrategies(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEventHandler<IndicatorUpdated>, IndicatorUpdatedHandler>();
        
        services.RegisterMapsterConfiguration();
        return services;
    }
}