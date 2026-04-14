using Microsoft.Extensions.Logging;

namespace MyTrades.EventSource.Retry;

public static class RetryExecutor
{
    public static async Task ExecuteAsync(
        Func<Task> action,
        RetryPolicyAttribute? policy,
        ILogger logger,
        string handlerName,
        string eventType,
        CancellationToken ct)
    {
        var maxAttempts = policy?.MaxAttempts ?? 1;
        var delayMs = policy?.DelayMs ?? 0;
        var exponential = policy?.UseExponentialBackoff ?? false;

        Exception? lastException = null;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex)
            {
                lastException = ex;

                logger.LogWarning(ex,
                    "Handler {Handler} failed on attempt {Attempt}/{MaxAttempts} for event {EventType}",
                    handlerName, attempt, maxAttempts, eventType);

                if (attempt >= maxAttempts)
                    break;

                var delay = exponential
                    ? delayMs * (int)Math.Pow(2, attempt - 1)
                    : delayMs;

                await Task.Delay(delay, ct);
            }
        }

        throw new HandlerException(handlerName, eventType, maxAttempts, lastException!);
    }
}