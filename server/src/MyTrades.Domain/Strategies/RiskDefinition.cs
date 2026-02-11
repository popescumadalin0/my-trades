namespace MyTrades.Domain.Strategies;

public sealed class RiskDefinition
{
    public decimal StopLoss { get; init; }
    public decimal TakeProfit { get; init; }
    public decimal MaxRiskPerTrade { get; init; }
}
