namespace MyTrades.EventSource.Events;

public record CandleCreated(
    string SymbolName,
    DateTimeOffset Timeframe,
    DateTimeOffset OpenTime,
    DateTimeOffset CloseTime,
    decimal OpenPrice,
    DateTimeOffset CreatedAt,
    decimal Volume,
    decimal TradeCount,
    decimal HighPrice,
    decimal LowPrice,
    decimal ClosePrice) : IEvent;