using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class FournisseurEuropeService<TFournisseurEurope> 
    : EuropeService<TFournisseurEurope, IFournisseurEuropeRepository<TFournisseurEurope>>, IFournisseurEuropeService 
    where TFournisseurEurope : ClientEurope, new()
{
    public FournisseurEuropeService(
        ICurrentContextProvider contextProvider, 
        IMapper mapper,
        IFournisseurEuropeRepository<TFournisseurEurope> repository,
        IGroupRepository groupRepository) 
        : base(contextProvider, repository, groupRepository, mapper)
    {
    }
}