using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyTrades.Gateway;

public static class DependencyInjection
{
    //todo: create clients;
    public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration config)
    {
        
        return services;
    }
}