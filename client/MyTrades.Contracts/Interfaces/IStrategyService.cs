using MyTrades.Contracts.Models;

namespace MyTrades.Contracts.Interfaces;

public interface IStrategyService
{
    public Task<ApiResponse<List<Strategy>>> GetStrategiesAsync();
    
    public Task<ApiResponse> AddOrUpdateStrategyAsync(Strategy strategy);
}