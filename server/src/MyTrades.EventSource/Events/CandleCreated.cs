namespace MyTrades.EventSource.Events;

public record CandleCreated(
    long Id,
    string SymbolName,
    DateTime Time,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    decimal Volume,
    decimal OpenInterest,
    decimal HighestPrice,
    decimal LowestPrice,
    decimal ClosePrice) : IEvent;