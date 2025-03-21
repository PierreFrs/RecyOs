// /** IIrPropertyService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 16/07/2023
//  * Fichier Modifié le : 16/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System.Threading.Tasks;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IIrPropertyService
{
    public Task<IrPropertyDto> UpdateProperty(IrPropertyDto prmProperty);
    public Task<IrPropertyDto> CreateProperty(IrPropertyDto prmProperty);
    public Task<IrPropertyDto> GetPropertyAsync(int id);
    public Task<IrPropertyDto[]> GetResPartnerPropertiesAsync(int resPartnerId);
    public Task<IrPropertyDto> GetResPartnerPropertyAsync(int resPartnerId, string propertyName);
}