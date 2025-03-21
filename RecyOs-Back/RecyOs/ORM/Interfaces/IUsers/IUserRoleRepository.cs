// /** IUserRoleRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IUserRoleRepository<TUserRole> where TUserRole : UserRole
{
    Task<TUserRole> Add(TUserRole userRole, ContextSession session);
    Task<TUserRole> Get(int userId, int roleId, ContextSession session);
    Task Delete(int userId, int roleId, ContextSession session);
    Task<IList<string>> GetByUserId(int userId, ContextSession session);
    
}