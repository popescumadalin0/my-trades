using MyTrades.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTrades.Client.Services
{
    public class MockDataService
    {
        public List<Strategy> Strategies { get; } = new();
        public List<Trade> Trades { get; } = new();

        public MockDataService()
        {
            // seed some strategies
            for (int i = 1; i <= 8; i++)
            {
                Strategies.Add(new Strategy
                {
                    Name = $"Strategy {i}",
                    WinRate = Math.Round(30.0 + i * 5 + (i % 3) * 2, 2),
                    TradesCount = 10 * i,
                    Profit = 1000m * i
                });
            }

            // seed trades
            var rnd = new Random(0);
            for (int i = 0; i < 15; i++)
            {
                var strat = Strategies[rnd.Next(Strategies.Count)];
                var entry = Math.Round((decimal)(rnd.NextDouble() * 100), 2);
                var current = entry + (decimal)((rnd.NextDouble() - 0.5) * 10);
                Trades.Add(new Trade
                {
                    StrategyName = strat.Name,
                    Symbol = "CFD" + rnd.Next(1, 10),
                    Entry = entry,
                    CurrentPrice = current,
                    StopLoss = entry - 5,
                    TakeProfit = entry + 10
                });
            }
        }
    }
}