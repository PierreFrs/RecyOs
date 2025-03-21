using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseCouvertureService
{
    Task<GridData<EntrepriseCouvertureDto>> GetDataForGrid(EntrepriseCouvertureGridFilter filter);
    Task<EntrepriseCouvertureDto> GetById(int id);
    Task<EntrepriseCouvertureDto> GetBySiren(string siren);
    Task<EntrepriseCouvertureDto> Edit(EntrepriseCouvertureDto dto);
    Task<EntrepriseCouvertureDto> Create(EntrepriseCouvertureDto dto);
}