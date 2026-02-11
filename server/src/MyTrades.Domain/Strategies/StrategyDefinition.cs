namespace MyTrades.Domain.Strategies;

public sealed class StrategyDefinition
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public StrategyMode Mode { get; init; } = StrategyMode.RuleBased;
    public ConditionGroup Entry { get; init; } = ConditionGroup.Empty;
    public ConditionGroup Exit { get; init; } = ConditionGroup.Empty;
    public ConditionGroup Filters { get; init; } = ConditionGroup.Empty;
    public RiskDefinition Risk { get; init; } = new();
    public int Version { get; init; } = 1;
}
