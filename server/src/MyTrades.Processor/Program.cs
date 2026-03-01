using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Domain;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainServices(builder.Configuration);

builder.Services.AddLogging(b =>
{
    var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext() // No need
        .WriteTo.Console() // No need
        .WriteTo.File("Logs/Log.txt") // No Need
        .CreateLogger();
    b.AddSerilog(loggerConfiguration, dispose: true);
});

builder.Services.AddMapster();

var app = builder.Build();

//migration runner
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    await runner.RunAsync();
}

app.Run();