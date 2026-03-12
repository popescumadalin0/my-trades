namespace MyTrades.Domain.Market;

public class Symbol : IEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Exchange { get; set; }
}