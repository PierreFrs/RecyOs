// /** IrPropertyService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 17/05/2023
//  * Fichier Modifié le : 20/07/2023
//  * Code développé pour le projet : RecyOs
//  */

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Repository;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.OdooDB.Services;

public class ResPartnerService<TResPartner> : IResPartnerService where TResPartner : ResPartnerOdooModel, new()
{
    protected readonly IResPartnerRepository<TResPartner> _resPartnerRepository;
    protected readonly IResCompanyService _resCompanyService;
    protected readonly IAcountAccountService _accountAccountService;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructeur de la classe ResPartnerService.
    /// </summary>
    /// <param name="resPartnerRepository">Le référentiel pour interagir avec les entités ResPartner dans le système.</param>
    /// <param name="resCompanyRepository">Le référentiel pour interagir avec les entités ResCompany dans le système.</param>
    /// <param name="accountAccountService">Le service compte</param>
    /// <param name="mapper">Mapper</param>
    /// <remarks>
    /// Ce constructeur initialise l'instance de ResPartnerService avec le référentiel ResPartner et l'objet IMapper fournis.
    /// </remarks>
    public ResPartnerService(IResPartnerRepository<TResPartner> resPartnerRepository,
        IResCompanyService resCompanyRepository,
        IAcountAccountService accountAccountService,
        IMapper mapper)
    {
        _resPartnerRepository = resPartnerRepository;
        _resCompanyService = resCompanyRepository;
        _accountAccountService = accountAccountService;
        _mapper = mapper;
    }

    /// <summary>
    /// Insère un nouveau partenaire dans le système en utilisant les informations fournies dans le DTO.
    /// </summary>
    /// <param name="prmPartner">Le DTO du partenaire contenant les informations à utiliser pour la création du nouveau partenaire.</param>
    /// <returns>Retourne un DTO du partenaire nouvellement créé, ou null si la création a échoué.</returns>
    /// <remarks>
    /// Cette méthode utilise l'objet IMapper pour convertir le DTO du partenaire en une entité ResPartner.
    /// L'entité est ensuite passée au référentiel pour créer le nouveau partenaire.
    /// Si un PaymentTermId est fourni dans le DTO, la méthode UpdatePartnerCustomPaymentTermId est appelée pour mettre à jour le partenaire avec ce terme de paiement.
    /// Le partenaire nouvellement créé est mappé à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<ResPartnerDto> InsertPartnerAsync(ResPartnerDto prmPartner)
    {
        var resPartner = _mapper.Map<TResPartner>(prmPartner);
        ResPartnerOdooModel resPartnerResult = await _resPartnerRepository.CreatePartner(resPartner);
        if (prmPartner.CustomerPaymentTermId != null)
        {
            await _resPartnerRepository.UpdatePartnerCustomPaymentTermId(resPartnerResult.Id, prmPartner.CustomerPaymentTermId.Value);
        }

        if (prmPartner.SupplierPaymentTermId != null)
        {
            await _resPartnerRepository.UpdateSupplierCustomPaymentTermId(resPartnerResult.Id, prmPartner.SupplierPaymentTermId.Value); 
        }

        if (prmPartner.SellAccount != null)
            {
                var resCompanyList = _resCompanyService.GetAllResCompanies();
                // pour chaque société, on met à jour le compte comptable
                foreach (var resCompany in resCompanyList)
                {
                    var accounts  = _accountAccountService.GetAccountByCode(prmPartner.SellAccount); // on récupère les compte comptable correspondant au code
                    var resAccount = accounts.FirstOrDefault(x => x.CompanyId == resCompany.Id);
                    
                    // si le compte comptable existe pour cette société, on met à jour le partenaire
                    if (resAccount != null)
                    {
                        // mettre à jour le compte comptable du partenaire
                        await _resPartnerRepository.UpdatePartnerSellAccount(resPartnerResult.Id, resAccount.Id, resCompany.Id);
                    }
                }
            }

        if (prmPartner.BuyAccount != null)
            {
                var resCompanyList = _resCompanyService.GetAllResCompanies();
                // pour chaque société, on met à jour le compte comptable
                foreach (var resCompany in resCompanyList)
                {
                    var accounts  = _accountAccountService.GetAccountByCode(prmPartner.BuyAccount); // on récupère les compte comptable correspondant au code
                    var resAccount = accounts.FirstOrDefault(x => x.CompanyId == resCompany.Id);
                    
                    // si le compte comptable existe pour cette société, on met à jour le partenaire
                    if (resAccount != null)
                    {
                        // mettre à jour le compte comptable du partenaire
                        await _resPartnerRepository.UpdatePartnerBuyAccount(resPartnerResult.Id, resAccount.Id, resCompany.Id);
                    }
                }
            }
       
        return _mapper.Map<ResPartnerDto>(resPartnerResult);
    }

    /// <summary>
    /// Met à jour un partenaire existant dans le système en utilisant les informations fournies dans le DTO.
    /// </summary>
    /// <param name="prmPartner">Le DTO du partenaire contenant les informations à utiliser pour la mise à jour du partenaire.</param>
    /// <returns>Retourne un DTO du partenaire mis à jour, ou null si la mise à jour a échoué.</returns>
    /// <remarks>
    /// Cette méthode utilise l'objet IMapper pour convertir le DTO du partenaire en une entité ResPartner.
    /// L'entité est ensuite passée au référentiel pour mettre à jour le partenaire.
    /// Si un PaymentTermId est fourni dans le DTO, la méthode UpdatePartnerCustomPaymentTermId est appelée pour mettre à jour le partenaire avec ce terme de paiement.
    /// Le partenaire mis à jour est mappé à un DTO qui est ensuite retourné.
    /// </remarks>
    public async Task<ResPartnerDto> UpdatePartnerAsync(ResPartnerDto prmPartner)
    {
        var resPartner = _mapper.Map<TResPartner>(prmPartner);
        var resPartnerResult = await _resPartnerRepository.UpdatePartner(resPartner);
        if (prmPartner.CustomerPaymentTermId != null)
        {
            await _resPartnerRepository.UpdatePartnerCustomPaymentTermId(resPartnerResult.Id, prmPartner.CustomerPaymentTermId.Value);
        }

        if (prmPartner.SupplierPaymentTermId != null)
        {
            await _resPartnerRepository.UpdateSupplierCustomPaymentTermId(resPartnerResult.Id, prmPartner.SupplierPaymentTermId.Value); 
        }
        
        if (prmPartner.SellAccount != null)
            {
                var resCompanyList = _resCompanyService.GetAllResCompanies();

                var resAccount = _accountAccountService.GetAccountByCode(prmPartner.SellAccount);
                
                // pour chaque société, on met à jour le compte comptable
                foreach (var resCompany in resCompanyList)
                {
                    var account = resAccount.FirstOrDefault(x => x.CompanyId == resCompany.Id);
                    if (account != null)
                    {
                        // mettre à jour le compte comptable du partenaire
                        await _resPartnerRepository.UpdatePartnerSellAccount(resPartnerResult.Id, account.Id, resCompany.Id);
                    }

                }
            }
        
        if(prmPartner.BuyAccount != null)
            {
                var resCompanyList = _resCompanyService.GetAllResCompanies();

                var resAccount = _accountAccountService.GetAccountByCode(prmPartner.BuyAccount);
                
                // pour chaque société, on met à jour le compte comptable
                foreach (var resCompany in resCompanyList)
                {
                    var account = resAccount.FirstOrDefault(x => x.CompanyId == resCompany.Id);
                    if (account != null)
                    {
                        // mettre à jour le compte comptable du partenaire
                        await _resPartnerRepository.UpdatePartnerBuyAccount(resPartnerResult.Id, account.Id, resCompany.Id);
                    }

                }
            }
        
        return _mapper.Map<ResPartnerDto>(resPartnerResult);
    }
}