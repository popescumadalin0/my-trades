using MyTrades.Contracts.Models;

namespace MyTrades.Contracts.Interfaces;

public interface IStrategyService
{
    public Task<List<Strategy>> GetStrategiesAsync();
    
    public Task AddOrUpdateStrategyAsync(Strategy strategy);
}