namespace MyTrades.EventSource.Events;

public record IndicatorUpdated(
    string Id,
    string Name) : IEvent;