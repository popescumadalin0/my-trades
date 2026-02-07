using AutoMapper;
using MyTrades.Client.Models;
using MyTrades.Contracts.Models;

namespace MyTrades.Client.Profiles;

public class TradeProfile : Profile
{
    public TradeProfile()
    {
        CreateMap<Trade, TradeModel>().ReverseMap();
    }
}
