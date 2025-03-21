using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
namespace RecyOs.ORM.MapProfile;

public class EtablissementFicheProfile : Profile
{
    public EtablissementFicheProfile()
    {
        CreateMap<EtablissementFiche, EtablissementFicheDto>().ReverseMap();

        CreateMap<EtablissementFiche, EtablissementFiche>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Siret, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}