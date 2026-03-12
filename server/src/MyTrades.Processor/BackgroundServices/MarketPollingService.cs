using MyTrades.Domain.Market;
using MyTrades.Persistence;

namespace MyTrades.Processor.BackgroundServices;

public class MarketPollingService : BackgroundService
{
    private readonly ISymbolProvider _symbolProvider;
    private readonly IEnumerable<IEntityStore<Candle>> _stores;
    private readonly ILogger<MarketPollingService> _logger;

    public MarketPollingService(IEnumerable<IEntityStore<Candle>> stores, ILogger<MarketPollingService> logger, ISymbolProvider symbolProvider, IHttpClientFactory httpFactory)
    {
        _stores = stores;
        _logger = logger;
        _symbolProvider = symbolProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await AlignToNextMinute(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var symbols = await _symbolProvider.GetActiveSymbolsAsync();

            var tasks = symbols.Select(s => FetchAndProcess(s, stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task FetchAndProcess(string symbol, CancellationToken ct)
    {
        try
        {
            /*var client = _httpFactory.CreateClient("MarketApi");

            var response = await client.GetAsync(
                $"api/candles?symbol={symbol}&interval=1m", ct);

            response.EnsureSuccessStatusCode();*/

            var json = await response.Content.ReadAsStringAsync(ct);

            var candle = Parse(json);

            await _stores.StoreAsync(candle, ct);

            await CandleChannel.Channel.Writer.WriteAsync(candle, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed for {Symbol}", symbol);
        }
    }

    private async Task AlignToNextMinute(CancellationToken token)
    {
        var now = DateTime.UtcNow;
        var delay = TimeSpan.FromSeconds(60 - now.Second);

        await Task.Delay(delay, token);
    }
}