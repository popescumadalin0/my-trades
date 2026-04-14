using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.EventSource.Retry;

namespace MyTrades.Indicator.Events;

[RetryPolicy(maxAttempts: 3, delayMs: 500, useExponentialBackoff: true)]
public class CandleCreatedHandler : IEventHandler<CandleCreated>
{
    public Task Handle(CandleCreated evt, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}