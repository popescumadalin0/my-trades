using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Services;

public class TradeService : ITradeService
{
    public Task<List<Trade>> GetTradesAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateTradeAsync(Trade trade)
    {
        throw new NotImplementedException();
    }
}