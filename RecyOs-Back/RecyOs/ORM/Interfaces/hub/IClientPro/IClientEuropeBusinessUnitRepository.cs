using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces;

public interface IClientEuropeBusinessUnitRepository<TClientEuropeBusinessUnit, TBusinessUnit> 
    where TClientEuropeBusinessUnit : ClientEuropeBusinessUnit, new()
    where TBusinessUnit : BusinessUnit, new()
{
    Task<TClientEuropeBusinessUnit> CreateAsync(ClientEuropeBusinessUnit clientEuropeBusinessUnit, ContextSession session);
    Task<IList<TBusinessUnit>> GetBusinessUnitsByClientEuropeIdAsync(int clientEuropeId, ContextSession session, bool includeDeleted = false);
    Task<TClientEuropeBusinessUnit> GetByClientEuropeIdAndBusinessUnitIdAsync(int clientEuropeId, int businessUnitId, ContextSession session, bool includeDeleted = false);
    Task<bool> DeleteAsync(int clientEuropeId, int businessUnitId, ContextSession session);
}