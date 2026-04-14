using FastEndpoints;
using Mapster;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

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

builder.Services.AddMapster();

var app = builder.Build();

app.UseFastEndpoints();
app.Run();