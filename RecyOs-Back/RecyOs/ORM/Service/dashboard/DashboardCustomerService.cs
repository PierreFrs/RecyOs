using System.Threading.Tasks;
using RecyOs.ORM.DTO.dashboard;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.dashboard;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.dashboard;
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public class DashboardCustomerService<TEtablissementClient, TEtablissementFiche, TEntrepriseCouverture, TEntrepriseNDCover> : BaseService,
    IDashboardCustomerService
    where TEtablissementClient : EtablissementClient, new()
    where TEtablissementFiche : EtablissementFiche, new()
    where TEntrepriseCouverture : EntrepriseCouverture, new()
    where TEntrepriseNDCover : EntrepriseNDCover, new()
{
    private readonly IEtablissementClientRepository<TEtablissementClient> _etablissementClientRepository;
    private readonly IEtablissementFicheRepository<TEtablissementFiche> _etablissementFicheRepository;
    private readonly IEntrepriseCouvertureRepository<TEntrepriseCouverture> _entrepriseCouvertureRepository;
    private readonly IEntrepriseNDCoverRepository<TEntrepriseNDCover> _entrepriseNDCoverRepository;
    private readonly ICommercialBaseRepository _commercialBaseRepository;
    public DashboardCustomerService(ICurrentContextProvider contextProvider,
        IEtablissementFicheRepository<TEtablissementFiche> etablissementFicheRepository,
        IEtablissementClientRepository<TEtablissementClient> etablissementClientRepository,
        IEntrepriseCouvertureRepository<TEntrepriseCouverture> entrepriseCouvertureRepository,
        IEntrepriseNDCoverRepository<TEntrepriseNDCover> entrepriseNDCoverRepository,
        ICommercialBaseRepository commercialBaseRepository
        ) : base(contextProvider)
    {
        _etablissementFicheRepository = etablissementFicheRepository;
        _etablissementClientRepository = etablissementClientRepository;
        _entrepriseCouvertureRepository = entrepriseCouvertureRepository;
        _entrepriseNDCoverRepository = entrepriseNDCoverRepository;
        _commercialBaseRepository = commercialBaseRepository;
    }

    public async Task<DashboardCustomerDto> GetDashboard()
    {
        int nbClients = 0;
        int nbBadMail = 0;
        int nbBadTel = 0;
        int nbRadies = 0;
        int nbFactor = 0;
        int nbDemCouv = 0;
        int nbDemCouvRefus = 0;
        int nbDemNDCov = 0;
        int ndDemNDCovRefus = 0;
        int nbCliSansCom = 0;

        nbClients = await GetNbClients();
        nbBadMail = await GetNbBadMail();
        nbBadTel = await GetNbBadTel();
        nbRadies = await GetNbRadies();
        nbFactor = await GetNbFactor();
        nbDemCouv = await GetNbDemCouv();
        nbDemCouvRefus = await GetNbDemCouvRefus();
        nbDemNDCov = await GetNbDemNDCov();
        ndDemNDCovRefus = await GetNbDemNDCovRefus();
        nbCliSansCom = await GetNbCliSansCom();

        var dashboard = new DashboardCustomerDto
        {
            NbClients = nbClients,
            NbRadies = nbRadies,
            NbErrMail = nbBadMail,
            NbErrTel = nbBadTel,
            NbFactor = nbFactor,
            NbFactorNc = -100, 
            NbDemCouv = nbDemCouv,
            NbDemCouvRefus = nbDemCouvRefus,
            NbDemNDCov = nbDemNDCov,
            NbDemNDCovRefus = ndDemNDCovRefus,
            NbCliSansCom = nbCliSansCom
        };
        return dashboard;
    }

    private async Task<int> GetNbClients()
    {
        EtablissementClientGridFilter filter = new EtablissementClientGridFilter()
        {
            PageSize = 0,
        };
        var tuple = await _etablissementClientRepository.GetFiltredListWithCount(filter, Session, false);
        return tuple.Item2;
    }

    private async Task<int> GetNbBadMail()
    {
        EtablissementClientGridFilter filter = new EtablissementClientGridFilter()
        {
            PageSize = 0,
            BadMail = "true"
        };
        var tuple = await _etablissementClientRepository.GetFiltredListWithCount(filter, Session, false);
        return tuple.Item2;
    }

    private async Task<int> GetNbBadTel()
    {
        EtablissementClientGridFilter filter = new EtablissementClientGridFilter()
        {
            PageSize = 0,
            BadTel = "true"
        };
        var tuple = await _etablissementClientRepository.GetFiltredListWithCount(filter, Session, false);
        return tuple.Item2;
    }

    private async Task<int> GetNbFactor()
    {
        EtablissementClientGridFilter filter = new EtablissementClientGridFilter()
        {
            PageSize = 0,
            Factor = "true"
        };
        var tuple = await _etablissementClientRepository.GetFiltredListWithCount(filter, Session, false);
        return tuple.Item2;
    }

    private async Task<int> GetNbRadies()
    {
        EtablissementFicheGridFilter filter = new EtablissementFicheGridFilter()
        {
            PageSize = 0,
            Radies = "true"
        };
        var tuple = await _etablissementFicheRepository.GetFiltredListWithCount(filter, Session, false);
        return tuple.Item2;
    }

    private async Task<int> GetNbDemCouv()
    {
        EntrepriseCouvertureGridFilter filter = new EntrepriseCouvertureGridFilter()
        {
            PageSize = 0
        };
        var tuple = await _entrepriseCouvertureRepository.GetFilteredListWithCount(filter, Session);
        return tuple.Item2;
    }
    
    private async Task<int> GetNbDemCouvRefus()
    {
        EntrepriseCouvertureGridFilter filter = new EntrepriseCouvertureGridFilter()
        {
            PageSize = 0,
            Refus = "true"
        };
        var tuple = await _entrepriseCouvertureRepository.GetFilteredListWithCount(filter, Session);
        return tuple.Item2;
    }
    
    private async Task<int> GetNbDemNDCov()
    {
        EntrepriseNDCoverGridFilter filter = new EntrepriseNDCoverGridFilter()
        {
            PageSize = 0
        };
        var tuple = await _entrepriseNDCoverRepository.GetFilteredListWithCount(filter, Session);
        return tuple.Item2;
    }
    
    private async Task<int> GetNbDemNDCovRefus()
    {
        EntrepriseNDCoverGridFilter filter = new EntrepriseNDCoverGridFilter()
        {
            PageSize = 0,
            Refus = "true"
        };
        var tuple = await _entrepriseNDCoverRepository.GetFilteredListWithCount(filter, Session);
        return tuple.Item2;
    }
    
    private async Task<int> GetNbCliSansCom()
    {
        Commercial? sansCommercialEntity = await _commercialBaseRepository.GetByMailAsync("null@null.null", new ContextSession());
        int sansCommercialId = 0;

        if (sansCommercialEntity != null)
        {
            sansCommercialId = sansCommercialEntity.Id;

            ClientByCommercialFilter filter = new ClientByCommercialFilter()
            {
                PageSize = 0,
            };
            var tuple = await _commercialBaseRepository.GetClientsByCommercialIdAsyncWithCount(sansCommercialId,
                filter);
            return tuple.Item2;
        }else
        {
            return 0;
        }
    }
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
}