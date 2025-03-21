// /** IUserRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface IUserRepository<TUser> where TUser : User
{
    Task Delete(int id, ContextSession session);
    Task<(IEnumerable<TUser>, int)> GetFilteredListWithTotalCount(UsersGridFilter filter, ContextSession session, bool includeDeleted = false);
    Task<TUser> GetByLogin(string login, ContextSession session, bool includeDeleted = false);
    Task<TUser> GetByEmail(string email, ContextSession session, bool includeDeleted = false);
    Task<TUser> Get(int id, ContextSession session, bool includeDeleted = false);
    Task<TUser> Edit(TUser user, ContextSession session);
}