using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.Gateway;
using MyTrades.Persistence;
using MyTrades.Processor.BackgroundServices;
using MyTrades.Processor.Contracts;
using MyTrades.Processor.EventHandlers;
using MyTrades.Processor.Profiles;

namespace MyTrades.Processor;

public static class DependencyInjection
{
    public static IServiceCollection RegisterProcessor(this IServiceCollection services, IConfiguration config)
    {
        services.RegisterMapsterConfiguration();

        services.AddHostedService<MarketPollingService>();

        services.AddHostedService<SymbolRefresher>();

        services.AddScoped<ISymbolLookup, SymbolLookup>();

        services.AddGatewayServices(config);

        services.AddScoped<IEventHandler<SymbolUpdated>, SymbolUpdatedHandler>();

        return services;
    }
}