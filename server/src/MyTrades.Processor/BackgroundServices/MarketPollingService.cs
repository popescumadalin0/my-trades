using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    public MarketPollingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await AlignToNextMinute(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MarketPollingService>>();
            var symbolLookup = scope.ServiceProvider.GetRequiredService<ISymbolLookup>();

            logger.LogInformation($"Started {nameof(MarketPollingService)}");

            logger.LogDebug($"Fetching candles {nameof(MarketPollingService)}");

            var symbols = await symbolLookup.GetAllAsync();

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

            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            await eventBus.PublishAsync(@event);

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
        var delay = TimeSpan.FromSeconds(60 - now.Second);

        await Task.Delay(delay, token);
    }
}