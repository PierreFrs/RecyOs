// IBalanceParticulierRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceParticulierRepository : IBalanceRepository<BalanceParticulier>
{
    Task<(IEnumerable<BalanceParticulier>,int, decimal)> GetFilteredListWithCount(BalanceParticulierGridFilter filter, bool includeDeleted = false);
}