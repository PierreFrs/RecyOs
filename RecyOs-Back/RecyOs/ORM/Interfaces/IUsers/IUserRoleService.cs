// /** IRoleService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RecyOs.ORM.Interfaces;

public interface IUserRoleService
{
    Task<IdentityResult> AssignToRole(int userId, string roleName);
    Task<IdentityResult> UnassignRole(int userId, string roleName);
    Task<IList<string>> GetRoles(int userId);
}