using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces.hub;

public interface IGroupService : IBaseService<GroupDto>
{
    Task<GridData<GroupDto>> GetFilteredListWithClientsAsync(GroupFilter filter);
    Task<GroupDto> GetByNameAsync(string name);
} 