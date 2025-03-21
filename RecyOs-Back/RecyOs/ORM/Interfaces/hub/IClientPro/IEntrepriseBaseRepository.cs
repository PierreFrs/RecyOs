using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseBaseRepository<TEntrepriseBase> where TEntrepriseBase : EntrepriseBase
{
    Task Delete(int id, ContextSession session);
    Task<(IEnumerable<TEntrepriseBase>,int)> GetFiltredListWithCount(EntrepriseBaseGridFilter filter, ContextSession session, bool includeDeleted = false);
    Task<TEntrepriseBase> GetBySiren(string siren, ContextSession session, bool includeDeleted = false);
    Task<TEntrepriseBase> Get(int id, ContextSession session, bool includeDeleted = false);
    Task<TEntrepriseBase> Update(TEntrepriseBase entrepriseBase, ContextSession session);
    Task<IEnumerable<TEntrepriseBase>> GetList(ContextSession session, bool includeDeleted = false);
    Task<TEntrepriseBase> Create(TEntrepriseBase entrepriseBase, ContextSession session);
    Task<bool> Exists(TEntrepriseBase obj, ContextSession session);
}