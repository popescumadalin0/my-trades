using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Client.Contracts;
using MyTrades.Client.Profiles;
using MyTrades.Client.Services;
using MyTrades.Contracts.Interfaces;

namespace MyTrades.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClientAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(x =>
        {
            x.AddProfile<TradeProfile>();
            x.AddProfile<StrategyProfile>();
        });

        return services;
    }

    public static IServiceCollection AddClientUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStrategyModelService, StrategyModelService>();
        services.AddScoped<ITradeModelService, TradeModelService>();

        return services;
    }

    public static IServiceCollection AddClientApiServices(this IServiceCollection services)
    {
        services.AddScoped<IStrategyService, StrategyHttpService>();
        services.AddScoped<ITradeService, TradeHttpService>();

        return services;
    }
}
