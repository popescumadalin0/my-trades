using System;

namespace MyTrades.Client.Models;

public class TradeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StrategyName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal Entry { get; set; }
    public decimal StopLoss { get; set; }
    public decimal TakeProfit { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal Pnl => CurrentPrice - Entry;
}