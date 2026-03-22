using System;
using JasperFx.Core.IoC;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.EventSource.BackgroundService;

namespace MyTrades.EventSource;

public static class DependencyInjection
{
    public static IServiceCollection AddEventSourceServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMarten(options =>
            {
                // Establish the connection string to your Marten database
                options.Connection(config.GetConnectionString("Marten")!);

                // If you want the Marten controlled PostgreSQL objects
                // in a different schema other than "public"
                options.DatabaseSchemaName = "other";

                // There are of course, plenty of other options...
            })

// This is recommended in new development projects
            .UseLightweightSessions()

// If you're using Aspire, use this option *instead* of specifying a connection
// string to Marten
            .UseNpgsqlDataSource();

        services.AddHostedService<EventDispatcher>();

        services.AddTransient<IEventBus, InMemoryEventBus>();

        services.RegisterEventHandlers();

        return services;
    }

    private static void RegisterEventHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}