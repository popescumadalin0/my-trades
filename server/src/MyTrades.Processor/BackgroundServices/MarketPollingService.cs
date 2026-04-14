using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyTrades.Domain.Market;
using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.Gateway;
using MyTrades.Persistence.Contracts;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.BackgroundServices;

public class MarketPollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ISymbolLookup _symbolLookup;

    private readonly IEventBus _eventBus;

    public MarketPollingService(IServiceScopeFactory scopeFactory, IEventBus eventBus, ISymbolLookup symbolLookup)
    {
        _scopeFactory = scopeFactory;
        _eventBus = eventBus;
        _symbolLookup = symbolLookup;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await AlignToNextMinute(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MarketPollingService>>();

            logger.LogInformation($"Started {nameof(MarketPollingService)}");

            logger.LogDebug($"Fetching candles {nameof(MarketPollingService)}");

            var symbols = await _symbolLookup.GetAllAsync();

            var tasks = symbols.Select(s => FetchAndProcess(s, stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task FetchAndProcess(NameIdentifier symbol, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        try
        {
            var candleGatewayService = scope.ServiceProvider.GetRequiredService<ICandleGatewayService>();
            var candle = await candleGatewayService.GetCandlesAsync(symbol.Name, ct);

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var @event = mapper.Map<CandleCreated>(candle);

            await _eventBus.PublishAsync(@event);

            var candleEntity = mapper.Map<Candle>(candle);

            candleEntity.SymbolId = symbol.Id;

            var stores = scope.ServiceProvider.GetRequiredService<IEnumerable<IStore<Candle>>>();
            foreach (var store in stores)
            {
                await store.StoreAsync(candleEntity, ct);
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MarketPollingService>>();
            logger.LogCritical(ex, "Failed for {Symbol}", symbol);
        }
    }

    private static async Task AlignToNextMinute(CancellationToken token)
    {
        var now = DateTime.UtcNow;
        var secondsUntilNextMinute = now.Second == 0 ? 0 : 60 - now.Second;
        await Task.Delay(TimeSpan.FromSeconds(secondsUntilNextMinute), token);
    }
}