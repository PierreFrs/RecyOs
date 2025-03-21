using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseBaseService
{
    Task<GridData<EntrepriseBaseDto>> GetDataForGrid(EntrepriseBaseGridFilter filter, bool includeDeleted = false);
    Task<EntrepriseBaseDto> GetById(int id, bool includeDeleted = false);
    Task<EntrepriseBaseDto> GetBySiren(string siren, bool includeDeleted = false);
    Task<bool> Delete(int id);
    Task<EntrepriseBaseDto> Edit(EntrepriseBaseDto dto);
    Task<EntrepriseBaseDto> Create(EntrepriseBaseDto dto);
}