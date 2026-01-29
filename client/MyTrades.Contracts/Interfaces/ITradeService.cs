using MyTrades.Contracts.Models;

namespace MyTrades.Contracts.Interfaces;

public interface ITradeService
{
    public Task<ApiResponse<List<Trade>>> GetTradesAsync();
    
    public Task<ApiResponse> AddOrUpdateTradeAsync(Trade trade);
}