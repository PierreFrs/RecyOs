// /** ResPartnerProfile.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 15/05/2023
//  * Fichier Modifié le : 30/06/2023
//  * Code développé pour le projet : RecyOs
//  */
using AutoMapper;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Mappers;

public class ResPartnerProfile: Profile
{
    public ResPartnerProfile()
    {
        CreateMap<ResPartnerOdooModel, ResPartnerDto>()
            .ForMember( dest => dest.CodeMkgt, opt => opt.MapFrom( src => src.XStudioCodeTiersMkgt ))
            .ForMember( dest => dest.Origine, opt => opt.MapFrom( src => src.XStudioOrigine ));
        CreateMap<ResPartnerDto, ResPartnerOdooModel>()
            .ForMember( dest => dest.XStudioCodeTiersMkgt, opt => opt.MapFrom( src => src.CodeMkgt ))
            .ForMember( dest => dest.XStudioOrigine, opt => opt.MapFrom( src => src.Origine ));
    }
}