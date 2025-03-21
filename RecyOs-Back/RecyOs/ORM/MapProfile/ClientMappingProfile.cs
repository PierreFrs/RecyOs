// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ClientMappingProfile.cs
// Created : 2024/03/27 - 10:56
// Updated : 2024/03/27 - 10:56

using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<EtablissementClient, ClientCompositeDto>();
        CreateMap<ClientEurope, ClientCompositeDto>();
    }
}