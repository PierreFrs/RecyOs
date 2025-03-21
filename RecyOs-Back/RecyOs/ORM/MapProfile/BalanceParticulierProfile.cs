// BalanceParticulierProfile.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.MapProfile;

public class BalanceParticulierProfile : Profile
{
    public BalanceParticulierProfile()
    {
        CreateMap<BalanceParticulier, BalanceDto>().ReverseMap();
        CreateMap<BalanceParticulier, BalanceParticuliersDto>()
            .ForMember(dest => dest.ClientParticulier, opt => opt.MapFrom(src => src.ClientParticuliers))
            .ForMember(dest => dest.Societe, opt => opt.MapFrom(src => src.Societe));
    }
}