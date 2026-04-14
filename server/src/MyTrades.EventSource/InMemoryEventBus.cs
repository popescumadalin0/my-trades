using System.Threading.Channels;

namespace MyTrades.EventSource;

public interface IEventBus
{
    ValueTask PublishAsync(IEvent evt);
    IAsyncEnumerable<IEvent> ReadAllAsync(CancellationToken ct);
}

public class InMemoryEventBus : IEventBus
{
    private readonly Channel<IEvent> _channel;

    public InMemoryEventBus()
    {
        _channel = Channel.CreateBounded<IEvent>(
            new BoundedChannelOptions(1000)
            {
                SingleReader = false,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.Wait
            });
    }

    public ValueTask PublishAsync(IEvent evt)
    {
        return _channel.Writer.WriteAsync(evt);
    }

    public IAsyncEnumerable<IEvent> ReadAllAsync(CancellationToken ct)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}