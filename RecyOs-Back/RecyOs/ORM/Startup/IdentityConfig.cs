// /** IdentityConfig.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;



public static class IdentityConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 4;
            options.User.RequireUniqueEmail = true;
        });

        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddTransient<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddTransient<IdentityErrorDescriber>();

        services.AddTransient<IRoleStore<Role>, RoleStore<Role>>();
        services.AddTransient<IUserStore<User>, UserStore<User, Role, UserRole, UserClaim>>();
        services.AddTransient<UserManager<User>, ApplicationUserManager>();
        services.AddTransient(typeof(RoleManager<Role>));

        var identityBuilder = new IdentityBuilder(typeof(User), typeof(User), services);
        identityBuilder.AddDefaultTokenProviders();
    }
}