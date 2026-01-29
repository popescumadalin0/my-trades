using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyTrades.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddClientServices(builder.Configuration);

await builder.Build().RunAsync();
