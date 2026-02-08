using MyTrades.Client.Models;

namespace MyTrades.Client.Contracts;

public interface IStrategyService
{
    public Task<List<StrategyModel>> GetStrategiesAsync();
    
    public Task AddOrUpdateStrategyAsync(StrategyModel model);
}