namespace MyTrades.Domain;

[AttributeUsage(AttributeTargets.Class)]
public class TableNameAttribute : Attribute
{
    public string Name { get; }
    public TableNameAttribute(string name) => Name = name;
}