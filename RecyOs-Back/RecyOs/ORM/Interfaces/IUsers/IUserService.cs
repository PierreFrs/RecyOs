// /** IUserService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface IUserService
{
    Task<GridData<UserDto>> GetDataForGrid(UsersGridFilter filter, bool includeDeleted = false);
    Task<UserDto> GetById(int id, bool includeDeleted = false);
    Task<UserDto> GetByLogin(string login, bool includeDeleted = false);
    Task<bool> Delete(int id);
    Task<UserDto> Edit(UserDto dto);
    Task<UserDto> GetByEmail(string mail, bool includeDeleted = false);
    Task<UserDto> CreateByEmail(string mail, bool includeDeleted = false);
}