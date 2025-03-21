// IBalanceParticulierService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceParticulierService : IBalanceService<BalanceDto>
{
    Task<GridData<BalanceParticuliersDto>> GetDataForGrid(BalanceParticulierGridFilter filter, bool includeDeleted = false);
}