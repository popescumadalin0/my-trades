namespace MyTrades.EventSource.Events;

public record SymbolUpdated(string Id, string Name) : IEvent;