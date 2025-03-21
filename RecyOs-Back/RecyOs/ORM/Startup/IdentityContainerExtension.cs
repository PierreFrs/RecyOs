// /** IdentityContainerExtension.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Archimede.ORM
//  */

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service;
using RecyOs.ORM.Service.hub;

namespace RecyOs.ORM.Startup;

public static class IdentityContainerExtension
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService<User>>();
        services.AddTransient<IUserRepository<User>, UserRepository>();

        services.AddTransient<IIdentityUserRepository<User>, IdentityUserRepository>();

        services.AddTransient<IRoleRepository<Role>, RoleRepository>();
        services.AddTransient<IRoleService, RoleService<Role>>();
        services.AddTransient<IUserRoleRepository<UserRole>, UserRoleRepository>();
        services.AddTransient<IUserClaimRepository<UserClaim>, UserClaimRepository>();
    }
}
    