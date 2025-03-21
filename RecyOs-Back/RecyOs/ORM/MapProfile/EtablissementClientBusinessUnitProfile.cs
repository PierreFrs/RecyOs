// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EtablissementClientBusinessUnitProfile.cs
// Created : 2024/01/24 - 11:47
// Updated : 2024/01/24 - 11:48

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class EtablissementClientBusinessUnitProfile : Profile
{
    public EtablissementClientBusinessUnitProfile()
    {
        CreateMap<EtablissementClientBusinessUnit, EtablissementClientBusinessUnitDto>();
        CreateMap<EtablissementClientBusinessUnitDto, EtablissementClientBusinessUnit>();
    }
}