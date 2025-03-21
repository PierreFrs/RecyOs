using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;

namespace RecyOs.ORM.Interfaces;

public interface IBusinessUnitService<TBusinessUnitDto>
{
    Task<List<TBusinessUnitDto>> GetListAsync();
    Task<TBusinessUnitDto> GetByIdAsync(int id);
}