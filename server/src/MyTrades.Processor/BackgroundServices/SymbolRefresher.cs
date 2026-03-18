using MyTrades.Domain.Market;
using MyTrades.Persistence.Contracts;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.BackgroundServices;

public class SymbolRefresher : BackgroundService
{
    private readonly IRepositoryDriver<Symbol> _symbolRepository;
    private readonly ILogger<SymbolRefresher> _logger;
    private readonly ISymbolLookup _symbolLookup;

    public SymbolRefresher(IRepositoryDriver<Symbol> symbolRepository, ISymbolLookup symbolLookup,
        ILogger<SymbolRefresher> logger)
    {
        _symbolRepository = symbolRepository;
        _symbolLookup = symbolLookup;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Started {nameof(SymbolRefresher)}");
        var symbols = await _symbolRepository.GetAllAsync(stoppingToken);

        foreach (var symbol in symbols)
        {
            await _symbolLookup.StoreSymbolNameAsync(new NameIdentifier(symbol.Name, symbol.Id));
        }
    }
}