namespace MyTrades.EventSource.Retry;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RetryPolicyAttribute : Attribute
{
    public int MaxAttempts { get; }
    public int DelayMs { get; }
    public bool UseExponentialBackoff { get; }

    public RetryPolicyAttribute(
        int maxAttempts = 3,
        int delayMs = 500,
        bool useExponentialBackoff = true)
    {
        MaxAttempts = maxAttempts;
        DelayMs = delayMs;
        UseExponentialBackoff = useExponentialBackoff;
    }
}