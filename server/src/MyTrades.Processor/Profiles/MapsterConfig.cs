using System.Reflection;
using Mapster;
using MyTrades.Domain.Market;
using MyTrades.Gateway.Refit.Responses;

namespace MyTrades.Processor.Profiles;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        //todo: map the name and the Id
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
            .Map(dest => dest.Time, src => src.Time);

        TypeAdapterConfig<CandleResponse, Candle>
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
            .Map(dest => dest.Time, src => src.Time);
        
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}