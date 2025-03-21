// /** ResPartnerRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/05/2023
//  * Fichier Modifié le : 20/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.Engine.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Repository;

public class ResPartnerRepository : BaseOdooRepository, IResPartnerRepository<ResPartnerOdooModel>
{

    private readonly OdooRepository<ResPartnerOdooModel> _odooRepository;
    private readonly OdooRepository<IrPropertyOdooModel> _odooRepositoryIrProperty;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public ResPartnerRepository(IConfiguration configuration) : base(configuration)
    {
        _odooRepository = new OdooRepository<ResPartnerOdooModel>(_odooConfig);
        _odooRepositoryIrProperty = new OdooRepository<IrPropertyOdooModel>(_odooConfig);
    }

    /// <summary>
    /// Récupère un partenaire du système en fonction de son ID.
    /// </summary>
    /// <param name="id">L'ID du partenaire à récupérer.</param>
    /// <returns>Retourne le modèle ResPartnerOdooModel correspondant au partenaire, ou null si aucun partenaire n'est trouvé.</returns>
    /// <remarks>
    /// Cette méthode utilise un filtre Odoo pour rechercher un partenaire avec l'ID spécifié dans le référentiel Odoo.
    /// Si un partenaire correspondant est trouvé, son modèle est retourné. Sinon, null est retourné.
    /// </remarks>
    public async Task<ResPartnerOdooModel> GetPartnerAsync(int id)
    {
        var filter = OdooFilter<ResPartnerOdooModel>.Create()
            .Or()
            .EqualTo(x => x.Id, id);
        
        var res =await _odooRepository.Query()
            .Where(filter)
            .FirstOrDefaultAsync();
        
        return res.Value;
    }

    /// <summary>
    /// Met à jour les informations d'un partenaire dans le système à partir d'un modèle fourni.
    /// </summary>
    /// <param name="prmPartner">Le modèle ResPartnerOdooModel contenant les informations mises à jour du partenaire.</param>
    /// <returns>Retourne le modèle de partenaire mis à jour, ou null si la mise à jour a échoué.</returns>
    /// <remarks>
    /// Cette méthode initialise une nouvelle instance de ResPartnerOdooModel avec les informations fournies dans prmPartner.
    /// L'instance est ensuite passée à la méthode UpdateAsync pour mise à jour.
    /// Si la mise à jour réussit, le modèle prmPartner est retourné. Sinon, null est retourné.
    /// </remarks>
    public async Task<ResPartnerOdooModel> UpdatePartner(ResPartnerOdooModel prmPartner)
    {
        var model = OdooDictionaryModel.Create(() => new ResPartnerOdooModel()
        {
            Name = prmPartner.Name,
            Street = prmPartner.Street,
            Street2 = prmPartner.Street2,
            Zip = prmPartner.Zip,
            City = prmPartner.City,
            Siret = prmPartner.Siret,
            Email = prmPartner.Email,
            Phone = prmPartner.Phone,
            Mobile = prmPartner.Mobile,
            Vat = prmPartner.Vat,
            IsCompany = prmPartner.IsCompany,
            XStudioCodeTiersMkgt = prmPartner.XStudioCodeTiersMkgt,
            XStudioOrigine = prmPartner.XStudioOrigine,
            CountryId = prmPartner.CountryId,
        });
        
        var retour = await UpdateAsync(model, (int)prmPartner.Id);
        if (retour)
        {
            return prmPartner;
        }
        else return null;
    }
    
    /// <summary>
    /// Recherche des partenaires dans le système basé sur le filtre odoo fourni.
    /// </summary>
    /// <param name="filter">Le filtre Odoo à utiliser pour la recherche de partenaires.</param>
    /// <returns>Retourne un tableau de modèles ResPartnerOdooModel correspondant aux résultats de la recherche.</returns>
    /// <remarks>
    /// Cette méthode utilise le filtre fourni pour rechercher dans le référentiel Odoo.
    /// Les résultats de la recherche sont retournés sous forme de tableau de ResPartnerOdooModel.
    /// </remarks>
    public async Task<ResPartnerOdooModel[]> SearchPartner(OdooFilter filter)
    {
        var res = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();
        return res.Value;
    }

    /// <summary>
    /// Crée un nouveau partenaire dans le système à partir d'un modèle fourni.
    /// </summary>
    /// <param name="prmPartner">Le modèle ResPartnerOdooModel contenant les informations du partenaire à créer.</param>
    /// <returns>Retourne le modèle de partenaire avec l'ID attribué lors de la création.</returns>
    /// <remarks>
    /// Cette méthode initialise une nouvelle instance de ResPartnerOdooModel avec les informations fournies dans prmPartner.
    /// L'instance est ensuite passée à la méthode CreateAsync pour traitement.
    /// L'ID attribué lors de la création est ajouté au modèle prmPartner, qui est ensuite retourné.
    /// </remarks>
    public async Task<ResPartnerOdooModel> CreatePartner(ResPartnerOdooModel prmPartner)
    {
        var model = OdooDictionaryModel.Create(() => new ResPartnerOdooModel()
        {
            Name = prmPartner.Name,
            Street = prmPartner.Street,
            Street2 = prmPartner.Street2,
            Zip = prmPartner.Zip,
            City = prmPartner.City,
            Siret = prmPartner.Siret,
            Email = prmPartner.Email,
            Phone = prmPartner.Phone,
            Mobile = prmPartner.Mobile,
            Vat = prmPartner.Vat,
            IsCompany = prmPartner.IsCompany,
            XStudioCodeTiersMkgt = prmPartner.XStudioCodeTiersMkgt,
            XStudioOrigine = prmPartner.XStudioOrigine,
            CountryId = prmPartner.CountryId,
        });
        int result = await CreateAsync(model);
        prmPartner.Id = result;
        return prmPartner;
    }

    /// <summary>
    /// Met à jour l'ID de terme de paiement personnalisé pour un partenaire donné.
    /// </summary>
    /// <param name="partnerId">L'ID du partenaire dont le terme de paiement doit être mis à jour.</param>
    /// <param name="paymentTermId">L'ID du terme de paiement à assigner au partenaire.</param>
    /// <returns>Retourne toujours vrai après la mise à jour réussie de l'instance.</returns>
    /// <remarks>
    /// Cette méthode recherche d'abord une instance existante d'IrPropertyOdooModel pour le partenaire spécifié.
    /// Si aucune propriété n'est trouvée, elle crée une nouvelle instance avec le terme de paiement spécifié.
    /// Si une propriété est trouvée, elle met à jour toutes les instances associées.
    /// Si une instance générique n'est pas trouvée parmi les propriétés mises à jour, elle en crée une affin que
    /// que les conditions existent sur toutes les entitées du groupe.
    /// </remarks>
    public async Task<bool> UpdatePartnerCustomPaymentTermId(long partnerId, int paymentTermId)
    {
        _logger.Trace("Mise à jour du terme de paiement personnalisé pour le partenaire {0} avec le terme de paiement {1}", partnerId, paymentTermId);
        string resId = "res.partner," + partnerId;
        string value = "account.payment.term," + paymentTermId;

        var res = await _odooRepositoryIrProperty.Query()
            .Where(OdooFilter<IrPropertyOdooModel>.Create()
                .And()
                .EqualTo(x => x.Name, "property_payment_term_id")
                .EqualTo(x => x.ResId, resId)
            )
            .ToListAsync();

        // if no property, create one generic
        if (res.Value == null)
        {
            await this.CreateGenericPartnerCustomPaymentTermId(partnerId, paymentTermId);
        }
        // if property, update all
        else
        {
            bool generic = false;
            foreach (var irPropertyOdooModel in res.Value)
            {
                if(irPropertyOdooModel.CompanyId == null)
                {
                    generic = true;
                }
                
                // if property need to be updated
                if(irPropertyOdooModel.ValueReference != value)
                {
                    var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
                    {
                        ValueReference = value
                    }); 
                    await UpdateAsync(model, irPropertyOdooModel.Id);
                    Thread.Sleep(100);
                }
            }

            // if no generic, create one
            if (!generic)
            {
                await this.CreateGenericPartnerCustomPaymentTermId(partnerId, paymentTermId);
            }
        }
        return true;
    }
    
    /// <summary>
    /// Met a jour l'ID de compte de vente pour un partenaire donné.
    /// </summary>
    /// <param name="partnerId">L'ID du partenaire pour lequel le terme de paiement est défini.</param>
    /// <param name="accountId">L'ID du compte de vente à attribuer au partenaire.</param>
    /// <param name="companyId">L'ID de la société à laquelle le compte de vente est associé.</param>
    /// <returns>Retourne toujours vrai après la mise à jour réussie de l'instance.</returns>
    /// <remarks>
    /// Cette méthode recherche d'abord une instance existante d'IrPropertyOdooModel pour le partenaire spécifié.
    /// Si aucune propriété n'est trouvée, elle crée une nouvelle instance avec le compte de vente spécifié.
    /// </remarks>
    public async Task<bool> UpdatePartnerSellAccount(long partnerId, long accountId, long companyId)
    {
        _logger.Trace("Mise à jour du compte de vente pour le partenaire {0} avec le compte de vente {1} société {2}", partnerId, accountId, companyId);
        string resId = "res.partner," + partnerId;
        string value = "account.account," + accountId;

        var res = await _odooRepositoryIrProperty.Query()
            .Where(OdooFilter<IrPropertyOdooModel>.Create()
                .And()
                .EqualTo(x => x.Name, "property_account_receivable_id")
                .EqualTo(x => x.ResId, resId)
                .EqualTo( x => x.CompanyId, companyId)
            )
            .ToListAsync();

        // if no property, create one generic
        if ((res.Value != null) && (res.Value.Length == 0) )
        {
            await this.CreateGenericPartnerSellAccount(partnerId, accountId, companyId);
        }
        // if property, update all
        else
        {
            foreach (var irPropertyOdooModel in res.Value)
            {
                // if property need to be updated
                if(irPropertyOdooModel.ValueReference != value)
                {
                    var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
                    {
                        ValueReference = value
                    }); 
                    await UpdateAsync(model, irPropertyOdooModel.Id);
                    Thread.Sleep(100);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Updates the custom payment term ID for a supplier partner.
    /// </summary>
    /// <param name="partnerId">The ID of the partner.</param>
    /// <param name="paymentTermId">The ID of the payment term.</param>
    /// <returns>Returns true if the update was successful, otherwise false.</returns>
    public async Task<bool> UpdateSupplierCustomPaymentTermId(long partnerId, int paymentTermId)
    {
        _logger.Trace("Mise à jour du terme de paiement personnalisé pour le partenaire {0} avec le terme de paiement {1}", partnerId, paymentTermId);
        string resId = "res.partner," + partnerId;
        string value = "account.payment.term," + paymentTermId;
        
        var res = await _odooRepositoryIrProperty.Query()
            .Where(OdooFilter<IrPropertyOdooModel>.Create()
                .And()
                .EqualTo(x => x.Name, "property_supplier_payment_term_id")
                .EqualTo(x => x.ResId, resId)
            )
            .ToListAsync();
        
        // if no property, create one generic
        if (res.Value == null)
        {
            await this.CreateGenericSupplierPaymentTermId(partnerId, paymentTermId);
        }
        // if property, update all
        else
        {
            bool generic = false;
            foreach (var irPropertyOdooModel in res.Value)
            {
                if(irPropertyOdooModel.CompanyId == null)
                {
                    generic = true;
                }
                
                // if property need to be updated
                if(irPropertyOdooModel.ValueReference != value)
                {
                    var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
                    {
                        ValueReference = value
                    }); 
                    await UpdateAsync(model, irPropertyOdooModel.Id);
                    Thread.Sleep(100);
                }
            }

            // if no generic, create one
            if (!generic)
            {
                await this.CreateGenericSupplierPaymentTermId(partnerId, paymentTermId);
            }
        }
        return true;
    }

    /// <summary>
    /// Update the partner's buy account with the given account and company.
    /// </summary>
    /// <param name="partnerId">The ID of the partner.</param>
    /// <param name="accountId">The ID of the account.</param>
    /// <param name="companyId">The ID of the company.</param>
    /// <returns>Returns true if the update is successful, otherwise false.</returns>
    public async Task<bool> UpdatePartnerBuyAccount(long partnerId, long accountId, long companyId)
    {
        _logger.Trace("Mise à jour du compte de vente pour le partenaire {0} avec le compte de vente {1} société {2}", partnerId, accountId, companyId);
        string resId = "res.partner," + partnerId;
        string value = "account.account," + accountId;

        var res = await _odooRepositoryIrProperty.Query()
            .Where(OdooFilter<IrPropertyOdooModel>.Create()
                .And()
                .EqualTo(x => x.Name, "property_account_payable_id")
                .EqualTo(x => x.ResId, resId)
                .EqualTo( x => x.CompanyId, companyId)
            )
            .ToListAsync();
        
        // if no property, create one generic
        if ((res.Value != null) && (res.Value.Length == 0) )
        {
            await this.CreateGenericPartnerBuyAccount(partnerId, accountId, companyId);
        }
        // if property, update all
        else
        {
            foreach (var irPropertyOdooModel in res.Value)
            {
                // if property need to be updated
                if(irPropertyOdooModel.ValueReference != value)
                {
                    var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
                    {
                        ValueReference = value
                    }); 
                    await UpdateAsync(model, irPropertyOdooModel.Id);
                    Thread.Sleep(100);
                }
            }
        }
        return true;
    }
    
    /// <summary>
    /// Crée une instance d'ID de terme de paiement personnalisé générique pour un partenaire donné.
    /// </summary>
    /// <param name="partnerId">L'ID du partenaire pour lequel le terme de paiement est défini.</param>
    /// <param name="paymentTermId">L'ID du terme de paiement à attribuer au partenaire.</param>
    /// <returns>Retourne toujours vrai après la création réussie de l'instance.</returns>
    /// <remarks>
    /// Cette méthode crée une nouvelle instance d'IrPropertyOdooModel avec un type de propriété Many2one, en assignant les termes de paiement spécifiés au partenaire donné.
    /// Le modèle est ensuite passé à la méthode CreateAsync pour être traité.
    /// </remarks>
    private async Task<bool> CreateGenericPartnerCustomPaymentTermId(long partnerId, int paymentTermId)
    {
        _logger.Trace("Création du terme de paiement personnalisé pour le partenaire {0} avec le terme de paiement {1}", partnerId, paymentTermId);
        string resId = "res.partner," + partnerId;
        string value = "account.payment.term," + paymentTermId;
        
        var nmodel = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = "property_payment_term_id",   // nom de la propriété : délaie de paiement du client
            ValueReference = value,
            ResId = resId,
            FieldsId = 3265,
            Type = TypeIrPropertyOdoo.Many2one
        });
        await CreateAsync(nmodel);
        return true;
    }
    
    private async Task<bool> CreateGenericSupplierPaymentTermId(long partnerId, int paymentTermId)
    {
        _logger.Trace("Création du terme de paiement personnalisé pour le fournisseur {0} avec le terme de paiement {1}", partnerId, paymentTermId);
        string resId = "res.partner," + partnerId;
        string value = "account.payment.term," + paymentTermId;
        
        var nmodel = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = "property_supplier_payment_term_id",   // nom de la propriété : délaie de paiement du client
            ValueReference = value,
            ResId = resId,
            FieldsId = 3266,
            Type = TypeIrPropertyOdoo.Many2one
        });
        await CreateAsync(nmodel);
        return true;
    }

    private async Task<bool> CreateGenericPartnerBuyAccount(long partnerId, long accountId, long companyId)
    {
        _logger.Trace("Création du compte d'achat pour le partenaire {0} avec le compte d'achat {1} société {2}", partnerId, accountId, companyId);
        string resId = "res.partner," + partnerId;
        string value = "account.account," + accountId;
        
        var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = "property_account_payable_id",   // nom de la propriété : compte de vente du client
            ValueReference = value,
            ResId = resId,
            CompanyId = companyId,
            FieldsId = 3262,
            Type = TypeIrPropertyOdoo.Many2one
        });
        await CreateAsync(model);
        return true;
    }
    
    /// <summary>
    /// Crée une instance d'ID de compte de vente pour un partenaire donné.
    /// </summary>
    /// <param name="partnerId">L'ID du partenaire pour lequel le terme de paiement est défini.</param>
    /// <param name="accountId">L'ID du compte de vente à attribuer au partenaire.</param>
    /// <param name="companyId">L'ID de la société à laquelle le compte de vente est associé.</param>
    /// <returns>Retourne toujours vrai après la création réussie de l'instance.</returns>
    /// <remarks>
    /// Cette méthode crée une nouvelle instance d'IrPropertyOdooModel avec un type de propriété Many2one, en assignant le compte de vente spécifié au partenaire donné.
    /// Le modèle est ensuite passé à la méthode CreateAsync pour être traité.
    /// </remarks>
    private async Task<bool> CreateGenericPartnerSellAccount(long partnerId, long accountId, long companyId)
    {
        _logger.Trace("Création du compte de vente pour le partenaire {0} avec le compte de vente {1} société {2}", partnerId, accountId, companyId);
        string resId = "res.partner," + partnerId;
        string value = "account.account," + accountId;
        
        var model = OdooDictionaryModel.Create(() => new IrPropertyOdooModel()
        {
            Name = "property_account_receivable_id",   // nom de la propriété : compte de vente du client
            ValueReference = value,
            ResId = resId,
            CompanyId = companyId,
            FieldsId = 3263,
            Type = TypeIrPropertyOdoo.Many2one
        });
        await CreateAsync(model);
        return true;
    }
}