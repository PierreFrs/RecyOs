using AutoMapper;
using Microsoft.AspNetCore.Http;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class EtablissementClientRepository: EtablissementRepository, IEtablissementClientRepository<EtablissementClient>
{
  
    public EtablissementClientRepository(DataContext context, IHttpContextAccessor httpContextAccessor,
        IEntrepriseBaseRepository<EntrepriseBase> entrepriseBaseRepository, IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository, IMapper mapper) 
        : base(context, httpContextAccessor, entrepriseBaseRepository, etablissementFicheRepository, "Client")
    {
    }
}