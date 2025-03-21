using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces;

public interface ISocieteBaseRepository : IBaseRepository<Societe>
{
    Task<(IEnumerable<Societe>,int)> GetDataForGrid(SocieteGridFilter filter, ContextSession session, bool includeDeleted = false);
}