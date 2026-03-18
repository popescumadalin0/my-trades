using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyTrades.EventSource.BackgroundService;

public class EventDispatcher : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IEventBus _bus;
    private readonly IServiceProvider _sp;

    public EventDispatcher(IEventBus bus, IServiceProvider sp)
    {
        _bus = bus;
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var evt in _bus.ReadAllAsync(stoppingToken))
        {
            _ = ProcessEvent(evt, stoppingToken); // fire-and-forget
        }
    }

    private async Task ProcessEvent(IEvent evt, CancellationToken ct)
    {
        using var scope = _sp.CreateScope();

        var handlerType = typeof(IEventHandler<>).MakeGenericType(evt.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        var tasks = new List<Task>();

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("Handle");
            var task = (Task)method.Invoke(handler, new object[] { evt, ct })!;
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}