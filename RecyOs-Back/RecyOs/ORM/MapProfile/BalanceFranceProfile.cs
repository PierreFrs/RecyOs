// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceFranceProfile.cs
// Created : 2024/02/26 - 11:41
// Updated : 2024/02/26 - 11:41

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.MapProfile;

public class BalanceFranceProfile : Profile
{
    public BalanceFranceProfile()
    {
        CreateMap<BalanceFrance, BalanceDto>().ReverseMap();
        CreateMap<BalanceFrance, BalanceFranceDto>()
            .ForMember(dest => dest.EtablissementClient, opt => opt.MapFrom(src => src.EtablissementClient))
            .ForMember(dest => dest.Societe, opt => opt.MapFrom(src => src.Societe));
    }
}