using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.EventSource.Retry;

namespace MyTrades.Strategy.EventHandlers;

[RetryPolicy(maxAttempts: 3, delayMs: 500, useExponentialBackoff: true)]
public class IndicatorUpdatedHandler : IEventHandler<IndicatorUpdated>
{
    public Task Handle(IndicatorUpdated evt, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}