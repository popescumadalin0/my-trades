using AutoMapper;
using MyTrades.Client.Models;
using MyTrades.Contracts;

namespace MyTrades.Client.Profiles;

public class TradeProfile : Profile
{
    public TradeProfile()
    {
        CreateMap<Trade, TradeModel>().ReverseMap();
    }
}
