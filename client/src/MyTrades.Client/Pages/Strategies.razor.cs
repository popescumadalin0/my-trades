using Microsoft.AspNetCore.Components;
using MyTrades.Client.Contracts;
using MyTrades.Client.Models;

namespace MyTrades.Client.Pages;

public partial class Strategies
{
    [Inject] public IStrategyService StrategyService { get; set; }
    
    private List<StrategyModel> StrategyModels { get; set; }


    protected override async Task OnInitializedAsync()
    {
        StrategyModels = await StrategyService.GetStrategiesAsync();


        await base.OnInitializedAsync();
    }
}