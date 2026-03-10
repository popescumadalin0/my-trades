using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyTrades.Domain;

public static class DependencyInjection
{
    //todo: add polimorfism repository
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MigrationRunner>();
        
        return services;
    }
}