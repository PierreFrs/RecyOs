using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces;

public interface IBusinessUnitRepository<TBusinessUnit> where TBusinessUnit : BusinessUnit
{
    Task<IList<TBusinessUnit>> GetListAsync(ContextSession session, bool includeDeleted = false);
    Task<TBusinessUnit> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
}