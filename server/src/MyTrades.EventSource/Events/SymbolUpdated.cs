namespace MyTrades.EventSource.Events;

public record SymbolUpdated(long Id, string Name) : IEvent;