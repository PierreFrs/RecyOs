// /** IUserClaimRepository.cs -
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

public interface IUserClaimRepository<TUserClaim> where TUserClaim : UserClaim
{
    Task<IList<TUserClaim>> GetByUserId(int userId, ContextSession session);
    Task Delete(int userId, string claimType, string claimValue, ContextSession session);
    Task<TUserClaim> Add(TUserClaim userClaim, ContextSession session);
    Task<IList<TUserClaim>> EditMany(IList<TUserClaim> userClaims, ContextSession session);
    Task<IList<TUserClaim>> GetList(int userId, string claimType, string claimValue, ContextSession session);
}