using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseNDCoverRepository<TEntrepriseNDCover> where TEntrepriseNDCover : EntrepriseNDCover
{
    Task<(IEnumerable<TEntrepriseNDCover>,int)> GetFilteredListWithCount(EntrepriseNDCoverGridFilter filter, ContextSession session);
    Task<TEntrepriseNDCover> GetBySiren(string siren, ContextSession session);
    Task<TEntrepriseNDCover> Get(int id, ContextSession session);
    Task<TEntrepriseNDCover> Update(TEntrepriseNDCover entrepriseNDCover, ContextSession session);
    Task<bool> Exists(TEntrepriseNDCover obj, ContextSession session);
}