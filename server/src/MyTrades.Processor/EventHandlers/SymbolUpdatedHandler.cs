using MyTrades.EventSource;
using MyTrades.EventSource.Events;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor.EventHandlers;

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