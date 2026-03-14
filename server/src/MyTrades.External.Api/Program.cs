using System.Collections.Concurrent;
using MyTrades.External.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/candle", (string symbol = "BTCUSDT") =>
{
    var now = DateTime.UtcNow;
    var minuteKey = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

    var candle = MarketSimulator.GetCurrentCandle(symbol, minuteKey);

    return Results.Ok(candle);
});

app.Run();

namespace MyTrades.External.Api
{
    public record Candle(
        string Symbol,
        DateTime OpenTime,
        decimal Open,
        decimal High,
        decimal Low,
        decimal Close,
        decimal Volume
    );

    public static class MarketSimulator
    {
        private static readonly ConcurrentDictionary<string, Candle> Candles = new();
        private static readonly ConcurrentDictionary<string, decimal> LastClose = new();
        private static readonly Random Random = new();

        public static Candle GetCurrentCandle(string symbol, DateTime minuteKey)
        {
            var key = $"{symbol}_{minuteKey:yyyyMMddHHmm}";

            return Candles.GetOrAdd(key, _ =>
            {
                var previousClose = LastClose.GetOrAdd(symbol, _ => 50000m);

                var volatility = 0.002m; // 0.2% per minute
                var drift = (decimal)(Random.NextDouble() - 0.5) * 0.001m; // mic trend

                var changePercent = drift + (decimal)(Random.NextDouble() - 0.5) * volatility;
                var close = previousClose * (1 + changePercent);

                var high = Math.Max(previousClose, close) *
                           (1 + (decimal)Random.NextDouble() * 0.001m);

                var low = Math.Min(previousClose, close) *
                          (1 - (decimal)Random.NextDouble() * 0.001m);

                var volume = (decimal)(Random.NextDouble() * 100 + 10);

                var candle = new Candle(
                    symbol,
                    minuteKey,
                    decimal.Round(previousClose, 2),
                    decimal.Round(high, 2),
                    decimal.Round(low, 2),
                    decimal.Round(close, 2),
                    decimal.Round(volume, 2)
                );

                LastClose[symbol] = candle.Close;

                return candle;
            });
        }
    }
}