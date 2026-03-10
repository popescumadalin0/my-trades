namespace MyTrades.Processor;

public static class DependencyInjection
{
    public static IServiceCollection AddProcessorServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ISymbolProvider, SymbolProvider>();

        return services;
    }
}