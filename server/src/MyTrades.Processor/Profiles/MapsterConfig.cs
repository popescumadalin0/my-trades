using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Domain.Market;
using MyTrades.EventSource.Events;
using MyTrades.Gateway.Refit.Responses;

namespace MyTrades.Processor.Profiles;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<CandleResponse, Candle>
            .NewConfig()
            .Map(dest => dest.CloseTime, src => src.CloseTime)
            .Map(dest => dest.OpenTime, src => src.OpenTime)
            .Map(dest => dest.Volume, src => src.Volume)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.HighPrice, src => src.HighPrice)
            .Map(dest => dest.LowPrice, src => src.LowPrice)
            .Map(dest => dest.Timeframe, src => src.Timeframe)
            .Map(dest => dest.ClosePrice, src => src.ClosePrice)
            .Map(dest => dest.OpenPrice, src => src.OpenPrice)
            .Map(dest => dest.TradeCount, src => src.TradeCount)
            .Ignore(dest => dest.SymbolId)
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<CandleResponse, CandleCreated>
            .NewConfig()
            .Map(dest => dest.CloseTime, src => src.CloseTime)
            .Map(dest => dest.OpenTime, src => src.OpenTime)
            .Map(dest => dest.Volume, src => src.Volume)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.HighPrice, src => src.HighPrice)
            .Map(dest => dest.LowPrice, src => src.LowPrice)
            .Map(dest => dest.Timeframe, src => src.Timeframe)
            .Map(dest => dest.ClosePrice, src => src.ClosePrice)
            .Map(dest => dest.OpenPrice, src => src.OpenPrice)
            .Map(dest => dest.TradeCount, src => src.TradeCount)
            .Map(dest => dest.SymbolName, src => src.SymbolName);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}