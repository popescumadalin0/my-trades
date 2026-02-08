using AutoMapper;
using MyTrades.Client.Models;
using MyTrades.Contracts;

namespace MyTrades.Client.Profiles;

public class StrategyProfile : Profile
{
    public StrategyProfile()
    {
        CreateMap<Strategy, StrategyModel>().ReverseMap();
    }
}
