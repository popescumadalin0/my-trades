namespace MyTrades.Domain.Market;

[TableName("symbols")]
public class Symbol : IEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Exchange { get; set; }
}