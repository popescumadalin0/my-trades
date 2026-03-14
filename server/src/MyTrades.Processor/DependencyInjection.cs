using Mapster;
using MyTrades.Persistence;
using MyTrades.Processor.BackgroundServices;

namespace MyTrades.Processor;

public static class DependencyInjection
{
    public static IServiceCollection AddProcessorServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistenceServices(config);
        
        services.AddMapster();

        services.AddHostedService<MarketPollingService>();

        services.AddScoped<ISymbolProvider, SymbolProvider>();

        return services;
    }
}