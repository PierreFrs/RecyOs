using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces;

public interface ISocieteBaseService : IBaseService<SocieteDto>
{
    Task<GridData<SocieteDto>> GetDataForGrid(SocieteGridFilter filter, bool includeDeleted = false);
}