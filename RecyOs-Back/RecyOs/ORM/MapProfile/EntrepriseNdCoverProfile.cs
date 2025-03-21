// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EntrepriseNdCoverProfile.cs
// Created : 2023/12/19 - 10:52
// Updated : 2023/12/19 - 10:52

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class EntrepriseNdCoverProfile : Profile
{
    public EntrepriseNdCoverProfile()
    {
        CreateMap<EntrepriseNDCover, EntrepriseNDCoverDto>();
        CreateMap<EntrepriseNDCoverDto, EntrepriseNDCover>();
    }
    
}