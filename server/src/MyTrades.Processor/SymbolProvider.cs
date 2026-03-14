using MyTrades.Domain.Market;
using MyTrades.Persistence;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Processor;

public interface ISymbolProvider
{
    Task<IEnumerable<string>> GetAllSymbolsAsync();
}

public class SymbolProvider : ISymbolProvider
{
    private readonly IDbRepository<Symbol> _repository;

    public SymbolProvider(IDbRepository<Symbol> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> GetAllSymbolsAsync()
    {
        var symbols = await _repository.GetAllAsync();

        return symbols.Select(x=> x.Name).Distinct();
    }
}