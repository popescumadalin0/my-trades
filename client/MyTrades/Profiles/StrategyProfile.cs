using AutoMapper;
using MyTrades.Client.Models;
using MyTrades.Contracts.Models;

namespace MyTrades.Profiles;

public class StrategyProfile : Profile
{
    public StrategyProfile()
    {
        CreateMap<Strategy, StrategyModel>().ReverseMap();
    }
}