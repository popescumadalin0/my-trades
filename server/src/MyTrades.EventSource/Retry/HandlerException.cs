namespace MyTrades.EventSource.Retry;

public class HandlerException : Exception
{
    public string HandlerName { get; }
    public string EventType { get; }
    public int Attempts { get; }

    public HandlerException(string handlerName, string eventType, int attempts, Exception inner)
        : base($"Handler {handlerName} failed after {attempts} attempt(s) for event {eventType}", inner)
    {
        HandlerName = handlerName;
        EventType = eventType;
        Attempts = attempts;
    }
}