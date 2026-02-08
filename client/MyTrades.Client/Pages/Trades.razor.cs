using Microsoft.AspNetCore.Components;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;

namespace MyTrades.Client.Pages;

public partial class Trades
{
    private bool _createOpen = false;
    private TradeModel _newTradeModel = new TradeModel();
    
    [Inject] public ITradeService TradeService { get; set; }
    
    private List<TradeModel> TradeModels { get; set; }

    protected override async Task OnInitializedAsync()
    {
        TradeModels = await TradeService.GetTradesAsync();
        
        await base.OnInitializedAsync();
    }

    private void OpenNewTrade()
    {
        _newTradeModel = new TradeModel();
        _createOpen = true;
    }

    private void AddTrade()
    {
        TradeModels.Add(_newTradeModel);
        _createOpen = false;
    }
}