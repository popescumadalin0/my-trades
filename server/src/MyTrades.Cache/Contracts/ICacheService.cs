namespace MyTrades.Cache.Contracts;

public interface ICacheService
{ 
    Task<T> GetItem<T>(string key);
    Task SetItem<T>(string key, T item);
}