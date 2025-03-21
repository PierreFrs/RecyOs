// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceEuropeProfile.cs
// Created : 2024/02/26 - 14:30
// Updated : 2024/02/26 - 14:30

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.MapProfile;

public class BalanceEuropeProfile : Profile
{
    public BalanceEuropeProfile()
    {
        CreateMap<BalanceEurope, BalanceDto>().ReverseMap();
        CreateMap<BalanceEurope, BalanceEuropeDto>()
            .ForMember(dest => dest.ClientEurope, opt => opt.MapFrom(src => src.ClientEurope))
            .ForMember(dest => dest.Societe, opt => opt.MapFrom(src => src.Societe));
    }
}