namespace MyTrades.Domain.Market;

[TableName("candles")]
public class Candle : IEntity
{
    public long Id { get; set; }
    public long SymbolId { get; set; }
    public DateTimeOffset Timeframe { get; set; }
    public DateTimeOffset OpenTime { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public DateTimeOffset CloseTime { get; set; }
    public decimal Volume { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal OpenPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TradeCount { get; set; }
}