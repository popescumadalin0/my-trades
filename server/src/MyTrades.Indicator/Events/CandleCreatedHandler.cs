using MyTrades.EventSource;
using MyTrades.EventSource.Events;

namespace MyTrades.Indicator.Events;

public class CandleCreatedHandler : IEventHandler<CandleCreated>
{
    public Task Handle(CandleCreated evt, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}