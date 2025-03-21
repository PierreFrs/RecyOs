using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IRoleRepository <TRole> where TRole : Role
{
    Task Delete(int id, ContextSession session);
    Task<TRole> Get(int id, ContextSession session);
    Task<TRole> Get(string name, ContextSession session);
    Task<TRole> Edit(TRole role, ContextSession session);
    Task<IList<Role>> GetList(ContextSession session, bool includeDeleted = false);
    Task<IList<Role>> GetListByUserId(int userId, ContextSession session, bool includeDeleted = false);
}