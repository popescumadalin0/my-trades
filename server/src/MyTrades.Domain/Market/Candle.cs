namespace MyTrades.Domain.Market;

public sealed record 
    Candle(
    DateTimeOffset Time,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    decimal Volume);
