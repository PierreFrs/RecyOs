// Created by : Pierre FRAISSE
// RecyOs => RecyOs => SocieteProfile.cs
// Created : 2024/02/23 - 16:02
// Updated : 2024/02/23 - 16:02

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class SocieteProfile : Profile
{
    public SocieteProfile()
    {
        CreateMap<Societe, SocieteDto>();
        CreateMap<SocieteDto, Societe>();
        CreateMap<Societe, SocieteDtoUpdate>();
        CreateMap<SocieteDtoUpdate, Societe>();
    }
}