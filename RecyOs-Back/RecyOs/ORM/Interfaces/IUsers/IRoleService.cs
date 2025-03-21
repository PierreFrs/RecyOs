using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;

namespace RecyOs.ORM.Interfaces;

public interface IRoleService
{
    Task<RoleDto> GetRoleById(int id);
    Task<List<RoleDto>> GetRoles();
}