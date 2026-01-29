using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Contracts.Interfaces;
using MyTrades.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IStrategyService, StrategyService>();
        services.AddSingleton<ITradeService, TradeService>();
        
        return services;
    }
}