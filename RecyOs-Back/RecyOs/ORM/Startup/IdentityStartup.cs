// /** IdentityStartup.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 24/01/2021
//  * Fichier Modifié le : 24/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using RecyOs.Helpers;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapperConfiguration = AutoMapper.Configuration;

public class IdentityStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services,IConfiguration configuration)
    {
        IdentityConfig.Configure(services);
        services.ConfigureAuth(configuration);
        IdentityDependenciesConfig.ConfigureIdentityDependencies(services);
        RegisterMapping(services);
        services.AddAuthorization(opt => opt.RegisterPolicies());
    }

    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        IdentityStartup.ConfigureServices(services,null);
            
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
   
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
    //<summary>
    // Register all mapping profiles
    //</summary>
    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        IdentityStartup.Configure(app,env);
    }
        
    private static void RegisterMapping(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IdentityStartup));
    }
        
}