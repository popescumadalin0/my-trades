namespace MyTrades.EventSource;

public interface IEvent { }

public interface IEventHandler<T> where T : IEvent
{
    Task Handle(T @event, CancellationToken ct);
}