using Microsoft.AspNetCore.Components;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;

namespace MyTrades.Client.Pages;

public partial class Home
{
    [Inject] public IStrategyService StrategyService { get; set; }
    [Inject] public ITradeService TradeService { get; set; }

    private List<StrategyModel> Strategies { get; set; }
    private List<TradeModel> Trades { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Strategies = await StrategyService.GetStrategiesAsync();
        
        Trades = await TradeService.GetTradesAsync();
        

        await base.OnInitializedAsync();
    }
}