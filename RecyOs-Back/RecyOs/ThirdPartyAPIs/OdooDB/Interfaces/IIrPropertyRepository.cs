// /** IIrPropertyRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 15/07/2023
//  * Fichier Modifié le : 17/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IIrPropertyRepository<TIrProperty> where TIrProperty : IrPropertyOdooModel, new()
{
    Task<TIrProperty> GetPropertyAsync(int id);
    Task<TIrProperty> UpdateProperty(TIrProperty prmProperty);
    Task<TIrProperty> CreateProperty(TIrProperty prmProperty);
    Task<TIrProperty[]> SearchProperty(OdooFilter filter);
}