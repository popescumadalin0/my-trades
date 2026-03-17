using Mapster;
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
        
        services.AddSingleton<ISymbolLookup, SymbolLookup>();

        return services;
    }
}