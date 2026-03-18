namespace MyTrades.EventSource.Events;

public record SymbolUpdated(string Id, string Name) : IEvent;

//todo: create handlers for this event
public record CandleCreated(string Id, string Name) : IEvent;
