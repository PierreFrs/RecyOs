using AutoMapper;
using Microsoft.AspNetCore.Http;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class EtablissementFournisseurRepository: EtablissementRepository, IEtablissementFournisseurRepository<EtablissementClient>
{
    public EtablissementFournisseurRepository(DataContext context, IHttpContextAccessor httpContextAccessor,
        IEntrepriseBaseRepository<EntrepriseBase> entrepriseBaseRepository, 
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository, IMapper mapper) 
        : base(context, httpContextAccessor, entrepriseBaseRepository, etablissementFicheRepository,"Fournisseur")
    {
    }
}