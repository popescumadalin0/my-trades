using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Contracts.Interfaces;
using MyTrades.Profiles;
using MyTrades.Services;

namespace MyTrades;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(x =>
        {
            x.AddProfile<TradeProfile>();
            x.AddProfile<StrategyProfile>();
            
            x.LicenseKey = configuration["AutoMapper:LicenseKey"];
        });
        
        services.AddSingleton<IStrategyService, StrategyService>();
        services.AddSingleton<ITradeService, TradeService>();
        
        return services;
    }
}