using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MudBlazor;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Client.Services;

public class TradeModelService : ITradeModelService
{
    private readonly ITradeService _tradeService;

    private readonly ISnackbar _snackbar;

    private readonly IMapper _mapper;

    public TradeModelService(ITradeService tradeService, ISnackbar snackbar, IMapper mapper)
    {
        _tradeService = tradeService;
        _snackbar = snackbar;
        _mapper = mapper;
    }

    public async Task<List<TradeModel>> GetTradesAsync()
    {
        var strategies = await _tradeService.GetTradesAsync();

        if (!strategies.Success)
        {
            _snackbar.Add(strategies.ErrorMessage, Severity.Error);
            Console.WriteLine(strategies.ServerErrorMessage);
        }

        var response = _mapper.Map<List<TradeModel>>(strategies);
        return response;
    }

    public async Task AddOrUpdateTradeAsync(TradeModel model)
    {
        var trade = _mapper.Map<Trade>(model);
        var response = await _tradeService.AddOrUpdateTradeAsync(trade);

        if (!response.Success)
        {
            _snackbar.Add(response.ErrorMessage, Severity.Error);
            Console.WriteLine(response.ServerErrorMessage);
        }
    }
}