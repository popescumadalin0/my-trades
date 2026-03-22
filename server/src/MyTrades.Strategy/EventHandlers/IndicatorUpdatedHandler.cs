using MyTrades.EventSource;
using MyTrades.EventSource.Events;

namespace MyTrades.Strategy.EventHandlers;

public class IndicatorUpdatedHandler : IEventHandler<IndicatorUpdated>
{
    public Task Handle(IndicatorUpdated evt, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}