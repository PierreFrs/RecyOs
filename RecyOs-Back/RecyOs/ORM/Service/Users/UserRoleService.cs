// /** RoleService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class UserRoleService<TUser, TRole> : IUserRoleService
    where TUser : User, new()
    where TRole : Role, new()
{
    protected readonly UserManager<TUser> userManager;
    protected readonly RoleManager<TRole> roleManager;

    public UserRoleService(UserManager<TUser> userManager, RoleManager<TRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<IdentityResult> AssignToRole(int userId, string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return await userManager.AddToRoleAsync(user, roleName);
        }

        return IdentityResult.Failed(new IdentityError { Description = "Invalid role name" });
    }
    
    public async Task<IdentityResult> UnassignRole(int userId, string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return await userManager.RemoveFromRoleAsync(user, roleName);
        }

        return IdentityResult.Failed(new IdentityError { Description = "Invalid role name" });
    }

    public Task<IList<string>> GetRoles(int userId)
    {
        return userManager.GetRolesAsync(new TUser { Id = userId });
    }
}