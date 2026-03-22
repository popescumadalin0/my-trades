using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.Indicator.Events;
using MyTrades.Indicator.Profiles;

namespace MyTrades.Indicator;

public static class DependencyInjection
{
    public static IServiceCollection RegisterIndicators(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEventHandler<CandleCreated>, CandleCreatedHandler>();

        services.RegisterMapsterConfiguration();
        
        return services;
    }
}