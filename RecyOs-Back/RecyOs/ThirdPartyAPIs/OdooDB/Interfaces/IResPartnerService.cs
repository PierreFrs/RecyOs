// /** IResPartnerService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 17/05/2023
//  * Fichier Modifié le : 30/06/2023
//  * Code développé pour le projet : RecyOs
//  */
using System.Threading.Tasks;
using RecyOs.OdooDB;
using RecyOs.OdooDB.DTO;

namespace RecyOs.OdooDB.Interfaces;

public interface IResPartnerService
{
    public Task<ResPartnerDto> InsertPartnerAsync(ResPartnerDto prmPartner);
    public Task<ResPartnerDto> UpdatePartnerAsync(ResPartnerDto prmPartner);
}