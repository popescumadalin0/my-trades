using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using MyTrades;
using MyTrades.Components;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapGet("/api/strategies", async (IStrategyService strategyService) =>
    await strategyService.GetStrategiesAsync());

app.MapPost("/api/strategies", async (IStrategyService strategyService, Strategy strategy) =>
    await strategyService.AddOrUpdateStrategyAsync(strategy));

app.MapGet("/api/trades", async (ITradeService tradeService) =>
    await tradeService.GetTradesAsync());

app.MapPost("/api/trades", async (ITradeService tradeService, Trade trade) =>
    await tradeService.AddOrUpdateTradeAsync(trade));

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyTrades.Client._Imports).Assembly);

app.Run();
