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

public class StrategyModelService : IStrategyModelService
{
    private readonly IStrategyService _strategyService;

    private readonly ISnackbar _snackbar;

    private readonly IMapper _mapper;

    public StrategyModelService(IStrategyService strategyService, IMapper mapper, ISnackbar snackbar)
    {
        _strategyService = strategyService;
        _mapper = mapper;
        _snackbar = snackbar;
    }

    public async Task<List<StrategyModel>> GetStrategiesAsync()
    {
        var strategies = await _strategyService.GetStrategiesAsync();

        if (!strategies.Success)
        {
            _snackbar.Add(strategies.ErrorMessage, Severity.Error);
            Console.WriteLine(strategies.ServerErrorMessage);
        }

        var response = _mapper.Map<List<StrategyModel>>(strategies);
        return response;
    }

    public async Task AddOrUpdateStrategyAsync(StrategyModel model)
    {
        var strategy = _mapper.Map<Strategy>(model);
        var response = await _strategyService.AddOrUpdateStrategyAsync(strategy);

        if (!response.Success)
        {
            _snackbar.Add(response.ErrorMessage, Severity.Error);
            Console.WriteLine(response.ServerErrorMessage);
        }
    }
}