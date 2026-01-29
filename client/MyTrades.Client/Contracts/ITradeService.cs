using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Client.Models;

namespace MyTrades.Client.Contracts;

public interface ITradeModelService
{
    public Task<List<TradeModel>> GetTradesAsync();
    
    public Task AddOrUpdateTradeAsync(TradeModel trade);
}