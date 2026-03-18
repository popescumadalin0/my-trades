using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MyTrades.Domain.Market;
using MyTrades.Gateway.Refit.Responses;

namespace MyTrades.Processor.Profiles;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Candle, CandleResponse>
            .NewConfig()
            .Map(dest => dest.Close, src => src.Close)
            .Map(dest => dest.Open, src => src.Open)
            .Map(dest => dest.Volume, src => src.Volume)
            .Map(dest => dest.OpenInterest, src => src.OpenInterest)
            .Map(dest => dest.High, src => src.High)
            .Map(dest => dest.Low, src => src.Low)
            .Map(dest => dest.HighestPrice, src => src.Low)
            .Map(dest => dest.LowestPrice, src => src.Low)
            .Map(dest => dest.ClosePrice, src => src.ClosePrice)
            .Map(dest => dest.Time, src => src.Time)
            .Ignore(dest => dest.SymbolName)
            .Ignore(dest => dest.Id);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}