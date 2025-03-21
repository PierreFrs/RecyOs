using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class EntrepriseBaseProfile : Profile
{
    public EntrepriseBaseProfile()
    {
        CreateMap<EntrepriseBase, EntrepriseBaseDto>();
        CreateMap<EntrepriseBaseDto, EntrepriseBase>();
    }
}