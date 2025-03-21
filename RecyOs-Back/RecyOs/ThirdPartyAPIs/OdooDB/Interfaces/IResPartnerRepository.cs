// /** IResPartnerRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 17/05/2023
//  * Fichier Modifié le : 16/06/2023
//  * Code développé pour le projet : RecyOs
//  */
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IResPartnerRepository<TResPartner> where TResPartner : ResPartnerOdooModel, new()
{
    Task<TResPartner> GetPartnerAsync(int id);
    Task<TResPartner> UpdatePartner(TResPartner prmPartner);
    Task<TResPartner> CreatePartner(TResPartner prmPartner);
    Task<TResPartner[]> SearchPartner(OdooFilter filter);
    Task<bool> UpdatePartnerCustomPaymentTermId(long partnerId, int paymentTermId);
    Task<bool> UpdatePartnerSellAccount(long partnerId, long accountId, long companyId);
    Task<bool> UpdateSupplierCustomPaymentTermId(long partnerId, int paymentTermId);
    Task<bool> UpdatePartnerBuyAccount(long partnerId, long accountId, long companyId);
}