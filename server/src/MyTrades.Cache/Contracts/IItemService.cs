namespace MyTrades.Cache.Contracts;

public interface IItemService
{ 
    Task<T> GetItem<T>(string key);
    Task SetItem<T>(string key, T item);
}