namespace MyTrades.Domain.Strategies;

public sealed class ConditionGroup
{
    public static readonly ConditionGroup Empty = new();

    public LogicalOperator Operator { get; init; } = LogicalOperator.And;
    public IReadOnlyList<ConditionDefinition> Conditions { get; init; } = Array.Empty<ConditionDefinition>();
    public int RequiredConfirmations { get; init; } = 0;
    public decimal MinScore { get; init; } = 0m;
}
