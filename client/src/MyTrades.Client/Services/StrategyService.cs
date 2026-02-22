using AutoMapper;
using MudBlazor;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;
using MyTrades.Contracts;

namespace MyTrades.Client.Services;

public class StrategyService : IStrategyService
{
    private readonly ISnackbar _snackbar;

    private readonly IMapper _mapper;

    public StrategyService(IMapper mapper, ISnackbar snackbar)
    {
        _mapper = mapper;
        _snackbar = snackbar;
    }

    public async Task<List<StrategyModel>> GetStrategiesAsync()
    {
        var strategies = new ApiResponse<List<Strategy>>(new List<Strategy>()
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

        if (!strategies.Success)
        {
            _snackbar.Add(strategies.ErrorMessage, Severity.Error);
            Console.WriteLine(strategies.ServerErrorMessage);
        }

        var response = _mapper.Map<List<StrategyModel>>(strategies.Data);
        return response;
    }

    public async Task AddOrUpdateStrategyAsync(StrategyModel model)
    {
        var strategy = _mapper.Map<Strategy>(model);
      
        /*
        if (!response.Success)
        {
            _snackbar.Add(response.ErrorMessage, Severity.Error);
            Console.WriteLine(response.ServerErrorMessage);
        }*/
    }
}