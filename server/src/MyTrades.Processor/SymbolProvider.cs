using MyTrades.Cache.Contracts;


namespace MyTrades.Processor;

public interface ISymbolProvider
{
    Task<IList<string>> GetAllSymbolsAsync();
}

public class SymbolProvider : ISymbolProvider
{
    private readonly ICacheService _cacheService;

    public SymbolProvider(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<IList<string>> GetAllSymbolsAsync()
    {
        var symbols = await _cacheService.GetItem<List<string>>("symbols");

        return symbols;
    }
}