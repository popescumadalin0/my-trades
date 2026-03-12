using MyTrades.Domain.Market;
using MyTrades.Persistence;
using MyTrades.Persistence.Contracts;

namespace MyTrades.Processor;

public interface ISymbolProvider
{
    Task<IList<string>> GetAllSymbolsAsync();
}

public class SymbolProvider : ISymbolProvider
{
    private readonly IEntityStore<Symbol> _cacheRepository;

    public SymbolProvider(IEntityStore<Symbol> cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public async Task<IList<string>> GetAllSymbolsAsync()
    {
        var symbols = await _cacheRepository.GetItem<List<string>>("symbols");

        return symbols;
    }
}