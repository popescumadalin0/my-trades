using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Client.Contracts;
using MyTrades.Client.Services;
using MyTrades.Contracts.Interfaces;

namespace MyTrades.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IStrategyModelService, StrategyModelService>();
        services.AddSingleton<ITradeModelService, TradeModelService>();
        
        return services;
    }
}