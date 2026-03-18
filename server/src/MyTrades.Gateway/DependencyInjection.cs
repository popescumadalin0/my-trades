using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Gateway.Refit.Clients;
using Refit;

namespace MyTrades.Gateway;

public static class DependencyInjection
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMockApi(config);

        services.AddSingleton<ICandleGatewayService, CandleGatewayService>();


        return services;
    }

    private static IServiceCollection AddMockApi(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddRefitClient<IMockApi>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri(config["Gateway:MockApi:BaseUrl"]); });

        services.AddScoped<IMockApiClient, MockApiClient>();
        
        return services;
    }
}