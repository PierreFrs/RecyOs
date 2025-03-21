using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseCouvertureRepository<TEtablissementCouverture> where TEtablissementCouverture : EntrepriseCouverture
{
    Task<(IEnumerable<TEtablissementCouverture>,int)> GetFilteredListWithCount(EntrepriseCouvertureGridFilter filter, ContextSession session);
    Task<TEtablissementCouverture> GetBySiren(string siren, ContextSession session);
    Task<TEtablissementCouverture> Get(int id, ContextSession session);
    Task<TEtablissementCouverture> Update(TEtablissementCouverture entrepriseCouverture, ContextSession session);
    Task<bool> Exists(TEtablissementCouverture obj, ContextSession session);
}