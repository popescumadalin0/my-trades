using MyTrades.Client.Models;

namespace MyTrades.Client.Pages;

public partial class Trades
{
    private bool _createOpen = false;
    private TradeModel _newTradeModel = new TradeModel();

    private void OpenNewTrade()
    {
        _newTradeModel = new TradeModel();
        _createOpen = true;
    }

    private void AddTrade()
    {
        DataService.Trades.Add(_newTradeModel);
        _createOpen = false;
    }
}