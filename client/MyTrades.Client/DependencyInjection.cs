using MyTrades.Client.Contracts;
using MyTrades.Client.Profiles;
using MyTrades.Client.Services;

namespace MyTrades.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(x =>
        {
            x.AddProfile<TradeProfile>();
            x.AddProfile<StrategyProfile>();
        });

        return services;
    }

    public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStrategyService, StrategyService>();
        services.AddScoped<ITradeService, TradeService>();

        return services;
    }
}
