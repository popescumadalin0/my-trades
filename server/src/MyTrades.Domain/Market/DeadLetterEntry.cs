namespace MyTrades.Domain.Market;

[TableName("dead_letters")]
public class DeadLetterEntry : IEntity
{
    public long Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public string Handler { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public int Attempts { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}