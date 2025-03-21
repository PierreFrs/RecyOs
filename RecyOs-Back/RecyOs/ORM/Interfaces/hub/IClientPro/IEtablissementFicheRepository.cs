using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementFicheRepository<TEtablissementFiche> where TEtablissementFiche : EtablissementFiche
{
    Task Delete(int id, ContextSession session);
    Task<(IEnumerable<TEtablissementFiche>,int)> GetFiltredListWithCount(EtablissementFicheGridFilter filter, ContextSession session, bool includeDeleted = false);
    Task<TEtablissementFiche> GetBySiret(string siret, ContextSession session, bool includeDeleted = false);
    Task<TEtablissementFiche> Get(int id, ContextSession session, bool includeDeleted = false);
    Task<TEtablissementFiche> UpdateAsync(TEtablissementFiche etablissementFiche, ContextSession session);
    Task<IEnumerable<TEtablissementFiche>> GetList(ContextSession session, bool includeDeleted = false);
    Task<TEtablissementFiche> Create(TEtablissementFiche etablissementFiche, ContextSession session);
    Task<bool> Exists(TEtablissementFiche obj, ContextSession session, bool includeDeleted = false);    
}