using MyTrades.Contracts.Models;

namespace MyTrades.Contracts.Interfaces;

public interface ITradeService
{
    public Task<List<Trade>> GetTradesAsync();
    
    public Task AddOrUpdateTradeAsync(Trade trade);
}