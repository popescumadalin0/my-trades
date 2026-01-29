namespace MyTrades.Contracts.Models;

public class Strategy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double WinRate { get; set; }
    public int TradesCount { get; set; }
    public decimal Profit { get; set; }
}