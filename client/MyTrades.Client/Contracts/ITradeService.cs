using MyTrades.Client.Models;

namespace MyTrades.Client.Contracts;

public interface ITradeService
{
    public Task<List<TradeModel>> GetTradesAsync();
    
    public Task AddOrUpdateTradeAsync(TradeModel trade);
}