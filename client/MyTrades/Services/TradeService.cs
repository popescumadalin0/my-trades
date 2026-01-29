using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Services;

public class TradeService : ITradeService
{
    public async Task<ApiResponse<List<Trade>>> GetTradesAsync()
    {
        try
        {
            return new ApiResponse<List<Trade>>(new List<Trade>()
            {
                new Trade()
                {
                    Id = Guid.NewGuid(),
                    CurrentPrice = 1,
                    Entry = 1,
                    StopLoss = 2,
                    TakeProfit = 3,
                    StrategyName = "test",
                    Symbol = "AAPL",
                }
            });
        }
        catch (Exception e)
        {
            return new ApiResponse<List<Trade>>(e);
        }
    }

    public async Task<ApiResponse> AddOrUpdateTradeAsync(Trade trade)
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