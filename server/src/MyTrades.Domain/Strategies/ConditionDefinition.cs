namespace MyTrades.Domain.Strategies;

public sealed class ConditionDefinition
{
    public IndicatorType Indicator { get; init; } = IndicatorType.Rsi;
    public ConditionOperator Operator { get; init; } = ConditionOperator.LessThan;
    public decimal Value { get; init; }
    public decimal Weight { get; init; } = 1m;
    public IReadOnlyDictionary<string, decimal> Parameters { get; init; } = new Dictionary<string, decimal>();
}
