using Microsoft.AspNetCore.Components;
using MyTrades.Client.Models;

namespace MyTrades.Client.Pages;

public partial class StrategyEdit
{
    private void Save()
    {
        // in mock service the object is already updated (bound). In real app we'd call an API
    }

    [Parameter]
    public Guid Id { get; set; }

    private StrategyModel _strategy;

    /*protected override void OnInitialized()
    {
        _strategy = Strategies.FirstOrDefault(s => s.Id == Id);
    }*/
}