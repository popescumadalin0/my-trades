using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;
using MyTrades.Contracts.Interfaces;

namespace MyTrades.Client.Pages;

public partial class Home
{
    [Inject] public IStrategyModelService StrategyService { get; set; }
    [Inject] public ITradeModelService TradeService { get; set; }

    private List<StrategyModel> Strategies { get; set; }
    private List<TradeModel> Trades { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Strategies = await StrategyService.GetStrategiesAsync();
        
        Trades = await TradeService.GetTradesAsync();
        

        await base.OnInitializedAsync();
    }
}