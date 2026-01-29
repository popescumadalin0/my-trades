using System;

namespace MyTrades.Client.Models;

public class StrategyModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double WinRate { get; set; }
    public int TradesCount { get; set; }
    public decimal Profit { get; set; }
}