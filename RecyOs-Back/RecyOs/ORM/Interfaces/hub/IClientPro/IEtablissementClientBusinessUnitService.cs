using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces;

public interface IEtablissementClientBusinessUnitService<TEtablissementClientBusinessUnitDto, TBusinessUnitDto>
{
    Task<TEtablissementClientBusinessUnitDto> CreateAsync(EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto);
    Task<IList<TBusinessUnitDto>> GetByEtablissementClientIdAsync(int etablissementClientId, bool includeDeleted = false);
    Task<bool> DeleteAsync(EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto);
}