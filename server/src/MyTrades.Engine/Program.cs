using Mapster;
using MyTrades.EventSource;
using MyTrades.Indicator;
using MyTrades.Persistence;
using MyTrades.Processor;
using MyTrades.Strategy;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(b =>
{
    var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        /*.Enrich.FromLogContext() // No need
        .WriteTo.Console() // No need
        .WriteTo.File("Logs/Log.txt") // No Need*/
        .CreateLogger();
    b.AddSerilog(loggerConfiguration, dispose: true);
});

builder.Services.RegisterProcessor(builder.Configuration);
builder.Services.RegisterIndicators(builder.Configuration);
builder.Services.RegisterStrategies(builder.Configuration);

builder.Services.AddMapster();

builder.Services.AddEventSourceServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

//migration runner
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    await runner.RunAsync();
}

app.Run();