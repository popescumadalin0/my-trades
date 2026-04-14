namespace MyTrades.Processor.Contracts;

public interface ISymbolLookup
{
    Task<IEnumerable<NameIdentifier>> GetAllAsync();
    Task StoreSymbolNameAsync(NameIdentifier nameIdentifier);
}