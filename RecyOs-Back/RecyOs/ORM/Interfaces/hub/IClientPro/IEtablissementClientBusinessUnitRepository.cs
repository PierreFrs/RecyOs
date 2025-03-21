using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces;

public interface IEtablissementClientBusinessUnitRepository<TEtablissementClientBusinessUnit, TBusinessUnit>
    where TEtablissementClientBusinessUnit : EtablissementClientBusinessUnit, new()
    where TBusinessUnit : BusinessUnit, new()
{
    Task<TEtablissementClientBusinessUnit> CreateAsync(EtablissementClientBusinessUnit etablissementClientBusinessUnit, ContextSession session);
    Task<IList<TBusinessUnit>> GetBusinessUnitsByEtablissementClientIdAsync(int etablissementClientId, ContextSession session, bool includeDeleted = false);
    Task<TEtablissementClientBusinessUnit> GetByEtablissementClientIdAndBusinessUnitIdAsync(int etablissementClientId, int businessUnitId, ContextSession session, bool includeDeleted = false);
    Task<ServiceResult> UpdateClientIdInBUsAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session);
    Task<bool> DeleteAsync(int etablissementClientId, int businessUnitId, ContextSession session);
}