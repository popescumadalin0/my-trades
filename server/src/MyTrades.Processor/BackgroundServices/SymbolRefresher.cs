using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyTrades.Domain.Market;
using MyTrades.Persistence.Contracts;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.BackgroundServices;

public class SymbolRefresher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SymbolRefresher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SymbolRefresher>>();
        logger.LogInformation($"Started {nameof(SymbolRefresher)}");

        var symbolRepository = scope.ServiceProvider.GetRequiredService<IRepositoryDriver<Symbol>>();
        
        var symbols = await symbolRepository.GetAllAsync(stoppingToken);

        var symbolLookup = scope.ServiceProvider.GetRequiredService<ISymbolLookup>();
        foreach (var symbol in symbols)
        {
            await symbolLookup.StoreSymbolNameAsync(new NameIdentifier(symbol.Name, symbol.Id));
        }
    }
}