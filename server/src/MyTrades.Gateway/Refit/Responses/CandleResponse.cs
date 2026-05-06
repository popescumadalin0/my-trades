namespace MyTrades.Gateway.Refit.Responses;

public class CandleResponse
{
    public string SymbolName { get; set; }
    public string Timeframe { get; set; }
    public DateTimeOffset OpenTime { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public DateTimeOffset CloseTime { get; set; }
    public decimal Volume { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal OpenPrice { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int TradeCount { get; set; }
}