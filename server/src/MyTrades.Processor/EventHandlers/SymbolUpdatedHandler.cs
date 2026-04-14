using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.EventSource.Retry;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.EventHandlers;

[RetryPolicy(maxAttempts: 3, delayMs: 500, useExponentialBackoff: true)]
public class SymbolUpdatedHandler : IEventHandler<SymbolUpdated>
{
    private readonly ISymbolLookup _lookUp;

    public SymbolUpdatedHandler(ISymbolLookup lookUp)
    {
        _lookUp = lookUp;
    }

    public Task Handle(SymbolUpdated evt, CancellationToken ct)
    {
        _lookUp.StoreSymbolNameAsync(new NameIdentifier(evt.Name, evt.Id));
        return Task.CompletedTask;
    }
}