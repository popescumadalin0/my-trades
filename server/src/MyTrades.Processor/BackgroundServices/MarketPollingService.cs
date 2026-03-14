using MapsterMapper;
using MyTrades.Domain.Market;
using MyTrades.Gateway;
using MyTrades.Persistence;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Processor.BackgroundServices;

public class MarketPollingService : BackgroundService
{
    private readonly ISymbolProvider _symbolProvider;
    private readonly ICandleGatewayService _candleGatewayService;
    private readonly IEnumerable<IEntityStore<Candle>> _stores;
    private readonly ILogger<MarketPollingService> _logger;
    
    private readonly IMapper _mapper;

    public MarketPollingService(
        IEnumerable<IEntityStore<Candle>> stores,
        ILogger<MarketPollingService> logger,
        ISymbolProvider symbolProvider,
        ICandleGatewayService candleGatewayService, IMapper mapper)
    {
        _stores = stores;
        _logger = logger;
        _symbolProvider = symbolProvider;
        _candleGatewayService = candleGatewayService;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await AlignToNextMinute(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var symbols = await _symbolProvider.GetAllSymbolsAsync();

            var tasks = symbols.Select(s => FetchAndProcess(s, stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task FetchAndProcess(string symbol, CancellationToken ct)
    {
        try
        {
            var candle = await _candleGatewayService.GetCandlesAsync(symbol);

            foreach (var store in _stores)
            {
                var candleEntity = _mapper.Map<Candle>(candle);
                await store.StoreAsync(candleEntity, ct);
            }
            //todo
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