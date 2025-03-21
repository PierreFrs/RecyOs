using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementFicheService
{
    Task<GridData<EtablissementFicheDto>> GetDataForGrid(EtablissementFicheGridFilter filter, bool includeDeleted = false);
    Task<EtablissementFicheDto> GetById(int id, bool includeDeleted = false);
    Task<EtablissementFicheDto> GetBySiret(string siret, bool includeDeleted = false);
    Task<bool> Delete(int id);
    Task<EtablissementFicheDto> Edit(EtablissementFicheDto dto);
    Task<EtablissementFicheDto> Create(EtablissementFicheDto dto);
}