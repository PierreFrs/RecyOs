// /** IIdentityUserRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IIdentityUserRepository<TUser> where TUser : User
{
    Task Delete(int id, ContextSession session);
    Task<TUser> GetById(int id, ContextSession session, bool includeDeleted = false);
    Task<TUser> GetByLogin(string login, ContextSession session, bool includeDeleted = false);
    Task<IList<TUser>> GetUsersByRole(int roleId, ContextSession session, bool includeDeleted = false);
    Task<IList<TUser>> GetUsersByClaim(string claimType, string claimValue, ContextSession session, bool includeDeleted = false);
    Task<TUser> GetByEmail(string email, ContextSession session, bool includeDeleted = false);
    Task<TUser> Edit(TUser user, ContextSession session);
}