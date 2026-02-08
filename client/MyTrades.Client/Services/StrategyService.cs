using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Services;

public class StrategyService : IStrategyService
{
    public async Task<ApiResponse<List<Strategy>>> GetStrategiesAsync()
    {
        try
        {
            return new ApiResponse<List<Strategy>>(new List<Strategy>()
            {
                new Strategy()
                {
                    Id = Guid.NewGuid(),
                    Name = "something",
                    Profit = 1,
                    TradesCount = 2,
                    WinRate = 20
                }
            });
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Strategy>>(ex);
        }
    }

    public async Task<ApiResponse> AddOrUpdateStrategyAsync(Strategy strategy)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex);
        }
    }
}