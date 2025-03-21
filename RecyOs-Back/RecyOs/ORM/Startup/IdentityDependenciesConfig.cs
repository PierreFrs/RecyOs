// /** DependenciesConfig.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */
using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class IdentityDependenciesConfig
{
    public static void ConfigureIdentityDependencies(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentContextProvider, CurrentContextProvider>();

        services.AddSingleton<JwtManager>();

        IdentityContainerExtension.Initialize(services);

        services.AddTransient<IAuthenticationService, AuthenticationService<User>>();
        services.AddTransient<IUserRoleService, UserRoleService<User, Role>>();
           
    }
}