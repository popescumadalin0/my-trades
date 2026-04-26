namespace MyTrades.Gateway.Refit.Responses;

public class CandleResponse
{
    public long Id { get; set; }
    public long SymbolId { get; set; }
    public DateTime Timeframe { get; set; }
    public DateTime OpenTime { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public DateTime CloseTime { get; set; }
    public decimal Volume { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal OpenPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TradeCount { get; set; }
}