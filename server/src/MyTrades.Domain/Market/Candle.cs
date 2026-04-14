namespace MyTrades.Domain.Market;

[TableName("candles")]
public class Candle : IEntity
{
    public long Id { get; set; }
    public long SymbolId { get; set; }
    public DateTime Time { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
    public decimal OpenInterest { get; set; }
    public decimal HighestPrice { get; set; } 
    public decimal LowestPrice { get; set; }
    public decimal ClosePrice { get; set; }
}