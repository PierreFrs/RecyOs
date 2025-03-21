// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotEuropeClientProfile.cs
// Created : 2024/04/17 - 08:51
// Updated : 2024/04/17 - 08:51

using AutoMapper;
using RecyOs.HubSpotDB.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Modules.HubSpot.Interfaces;

public class HubSpotEuropeClientProfile : Profile
{
    public HubSpotEuropeClientProfile()
    {
        CreateMap<ClientEuropeDto, CompaniesDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdHubspot))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AdresseFacturation1))
            .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AdresseFacturation2))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.HubSpotOwnerId, opt => opt.MapFrom(src => src.Commercial.IdHubSpot))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.PaysFacturation))
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.DateCreationFicheRecyOs, opt => opt.MapFrom(src => src.CreateDate))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.CodeMkgt));
        
        CreateMap<CompaniesDto, ClientEuropeDto>()
            .ForMember(dest => dest.IdHubspot, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.Zip));
    }

}