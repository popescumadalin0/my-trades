using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.EventSource;
using MyTrades.Persistence;
using MyTrades.Processor.BackgroundServices;
using MyTrades.Processor.Contracts;
using MyTrades.Processor.Profiles;

namespace MyTrades.Processor;

public static class DependencyInjection
{
    public static IServiceCollection AddProcessorServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistenceServices(config);
        
        services.AddMapster();

        services.RegisterMapsterConfiguration();

        services.AddHostedService<MarketPollingService>();

        services.AddHostedService<SymbolRefresher>();
        
        services.AddSingleton<ISymbolLookup, SymbolLookup>();

        services.AddEventSourceServices(config);

        return services;
    }
}