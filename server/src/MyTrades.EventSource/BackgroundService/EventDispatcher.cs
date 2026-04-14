using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyTrades.EventSource.Retry;

namespace MyTrades.EventSource.BackgroundService;

public class EventDispatcher : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IEventBus _bus;
    private readonly IServiceProvider _sp;
    private readonly ILogger<EventDispatcher> _logger;

    public EventDispatcher(IEventBus bus, IServiceProvider sp, ILogger<EventDispatcher> logger)
    {
        _bus = bus;
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var evt in _bus.ReadAllAsync(stoppingToken))
        {
            _ = ProcessEvent(evt, stoppingToken).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    _logger.LogError(t.Exception,
                        "Unhandled failure processing event {EventType}",
                        evt.GetType().Name);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    private async Task ProcessEvent(IEvent evt, CancellationToken ct)
    {
        using var scope = _sp.CreateScope();

        var eventType = evt.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        var handlers = scope.ServiceProvider.GetServices(handlerType).ToList();

        if (handlers.Count == 0)
        {
            _logger.LogWarning("No handlers found for {EventType}", eventType.Name);
            return;
        }

        var tasks = handlers.Select(handler => ExecuteHandler(handler!, evt, ct));

        await Task.WhenAll(tasks);
    }

    private async Task ExecuteHandler(object handler, IEvent evt, CancellationToken ct)
    {
        var handlerType = handler.GetType();
        var eventType = evt.GetType();

        var policy = handlerType.GetCustomAttributes(typeof(RetryPolicyAttribute), false)
            .FirstOrDefault() as RetryPolicyAttribute;

        var method = handler.GetType()
            .GetMethod("Handle", [eventType, typeof(CancellationToken)]);

        if (method == null)
        {
            _logger.LogError("Handle method not found on {Handler}", handlerType.Name);
            return;
        }

        try
        {
            await RetryExecutor.ExecuteAsync(
                action: () => (Task)method.Invoke(handler, [evt, ct])!,
                policy: policy,
                logger: _logger,
                handlerName: handlerType.Name,
                eventType: eventType.Name,
                ct: ct);
        }
        catch (HandlerException ex)
        {
            _logger.LogError(ex,
                "Handler {Handler} exhausted all retries for event {EventType} after {Attempts} attempts",
                ex.HandlerName, ex.EventType, ex.Attempts);

            //todo: dead letter – urmează în pasul următor
        }
    }
}