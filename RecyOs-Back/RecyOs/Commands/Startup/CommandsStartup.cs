//  CommandsStartup.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Controllers;
using RecyOs.Cron;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Startup;

namespace RecyOs.Commands.Startup;

public class CommandsStartup : IBaseStartup
{

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICommandImportFcli, ImportFcli>();
        services.AddTransient<ICommandImportCouverture, ImportCouverture>();
        services.AddTransient<ICommandImportNDCover, ImportNDCover>();
        services.AddTransient<ICommandExportSoumissionNDCoverService, CommandExportSoumissionNDCoverService>();
        services.AddTransient<ICommandExportSoumissionNDCoverRepository<EntrepriseBase>, 
            CommandExportSoumissionNDCoverRepository>();
        services.AddTransient<SyncBalanceCron>();  // Fixme: A déplacer dans un module
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }
    
    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        CommandsStartup.ConfigureServices(services, null);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        CommandsStartup.Configure(app, env);
    }
}