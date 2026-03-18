using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyTrades.Processor.Contracts;

namespace MyTrades.Processor;

public class SymbolLookup : ISymbolLookup
{
    //{symbolName, Id}
    private ConcurrentDictionary<string, string> _symbolLookup;

    public Task<IEnumerable<NameIdentifier>> GetAllAsync()
    {
        if (_symbolLookup == null)
        {
            return null;
        }

        var list = _symbolLookup.Select(s => new NameIdentifier(s.Key, s.Value));

        return Task.FromResult(list);
    }

    public Task StoreSymbolNameAsync(NameIdentifier nameIdentifier)
    {
        _symbolLookup ??= new ConcurrentDictionary<string, string>();

        var value = _symbolLookup.AddOrUpdate(nameIdentifier.Name, nameIdentifier.Id,
            (key, oldValue) => nameIdentifier.Id);
        if (value != nameIdentifier.Id)
        {
            throw new ArgumentException($"Symbol {nameIdentifier.Name} already exists");
        }

        return Task.CompletedTask;
    }
}

public record NameIdentifier(string Name, string Id);