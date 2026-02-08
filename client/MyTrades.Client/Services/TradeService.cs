using AutoMapper;
using MudBlazor;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;
using MyTrades.Contracts;

namespace MyTrades.Client.Services;

public class TradeService : ITradeService
{
    private readonly ISnackbar _snackbar;

    private readonly IMapper _mapper;

    public TradeService(ISnackbar snackbar, IMapper mapper)
    {
        _snackbar = snackbar;
        _mapper = mapper;
    }

    public async Task<List<TradeModel>> GetTradesAsync()
    {
        var trades = new ApiResponse<List<Trade>>(new List<Trade>()
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
        
        if (!trades.Success)
        {
            _snackbar.Add(trades.ErrorMessage, Severity.Error);
            Console.WriteLine(trades.ServerErrorMessage);
        }

        var response = _mapper.Map<List<TradeModel>>(trades.Data);
        return response;
    }

    public async Task AddOrUpdateTradeAsync(TradeModel model)
    {
        var trade = _mapper.Map<Trade>(model);

        /*if (!response.Success)
        {
            _snackbar.Add(response.ErrorMessage, Severity.Error);
            Console.WriteLine(response.ServerErrorMessage);
        }*/
    }
}