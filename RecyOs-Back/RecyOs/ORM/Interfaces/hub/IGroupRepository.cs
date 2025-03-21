using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using System.Collections.Generic;
namespace RecyOs.ORM.Interfaces.hub;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<(IEnumerable<Group>, int)> GetFilteredListWithClientsAsync(GroupFilter filter, ContextSession session, bool includeDeleted = false);
    Task<Group> GetByNameAsync(string name, ContextSession session);
} 