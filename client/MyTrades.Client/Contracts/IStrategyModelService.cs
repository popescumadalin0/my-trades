using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Client.Models;

namespace MyTrades.Client.Contracts;

public interface IStrategyModelService
{
    public Task<List<StrategyModel>> GetStrategiesAsync();
    
    public Task AddOrUpdateStrategyAsync(StrategyModel model);
}