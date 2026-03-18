using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrades.Processor.Contracts;

public interface ISymbolLookup
{
    Task<IEnumerable<NameIdentifier>> GetAllAsync();
    //todo: call this from mediatR handler
    Task StoreSymbolNameAsync(NameIdentifier nameIdentifier);
}