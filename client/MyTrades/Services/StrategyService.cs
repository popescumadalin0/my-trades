using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Services;

public class StrategyService : IStrategyService
{
    public Task<List<Strategy>> GetStrategiesAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateStrategyAsync(Strategy strategy)
    {
        throw new NotImplementedException();
    }
}