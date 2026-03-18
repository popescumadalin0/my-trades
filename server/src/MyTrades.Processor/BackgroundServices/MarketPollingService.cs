using MapsterMapper;
using MyTrades.Domain.Market;
using MyTrades.Gateway;
using MyTrades.Persistence.Contracts;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.BackgroundServices;

public class MarketPollingService : BackgroundService
{
    private readonly ICandleGatewayService _candleGatewayService;
    private readonly IEnumerable<IStore<Candle>> _stores;
    private readonly ILogger<MarketPollingService> _logger;
    private readonly ISymbolLookup _symbolLookup;

    private readonly IMapper _mapper;

    public MarketPollingService(
        IEnumerable<IStore<Candle>> stores,
        ILogger<MarketPollingService> logger,
        ICandleGatewayService candleGatewayService,
        IMapper mapper,
        ISymbolLookup symbolLookup)
    {
        _stores = stores;
        _logger = logger;
        _candleGatewayService = candleGatewayService;
        _mapper = mapper;
        _symbolLookup = symbolLookup;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Started {nameof(MarketPollingService)}");

        await AlignToNextMinute(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug($"Fetching candles {nameof(MarketPollingService)}");

            var symbols = await _symbolLookup.GetAllAsync();

            var tasks = symbols.Select(s => FetchAndProcess(s, stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task FetchAndProcess(NameIdentifier symbol, CancellationToken ct)
    {
        try
        {
            var candle = await _candleGatewayService.GetCandlesAsync(symbol.Name, ct);

            var candleEntity = _mapper.Map<Candle>(candle);

            candleEntity.SymbolId = symbol.Id;

            foreach (var store in _stores)
            {
                await store.StoreAsync(candleEntity, ct);
            }
            //todo: call mediator
            //await CandleChannel.Channel.Writer.WriteAsync(candle, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed for {Symbol}", symbol);
        }
    }

    private static async Task AlignToNextMinute(CancellationToken token)
    {
        var now = DateTime.UtcNow;
        var delay = TimeSpan.FromSeconds(60 - now.Second);

        await Task.Delay(delay, token);
    }
}