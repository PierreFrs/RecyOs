//  EngineStartup.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Cron;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Services;
using RecyOs.Engine.Modules.HubSpot.Interfaces;
using RecyOs.Engine.Modules.HubSpot.Services;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.Engine.Repository;
using RecyOs.Engine.Services;
using RecyOs.Engine.Modules.Mkgt.Services;
using RecyOs.Engine.Modules.Odoo.Services;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository.Application;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces.ICron;
using RecyOs.ORM.Interfaces.IParameters;
using RecyOs.ORM.Service;
using RecyOs.ORM.Startup;

namespace RecyOs.Engine.Startup;

public static class SynchroStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ISynchroWaitingToken, SynchroWaitingToken>(); // Ajout du jeton d'attente attention singleton
        services.AddHostedService<SynchroService>();                     // Ajout du service de synchro des données 
        services.AddSingleton<IEngineEtablissementClientRepository, EngineEtablissementClientRepository>(); // Ajout du repository des établissements clients
        services.AddSingleton<IEngineEtablissementClientService, EngineEtablissementClientService>(); // Ajout du service des établissements clients
        services.AddSingleton<IEngineEuropeClientRepository, EngineEuropeClientRepository>(); // Ajout du repository des établissements clients Europe
        services.AddSingleton<IEngineEuropeClientService, EngineEuropeClientService>(); // Ajout du service des établissements clients Europe
        services.AddSingleton<IMkgtClientService, MkgtClientService>();
        services.AddSingleton<IOdooClientService, OdooClientService>();
        services.AddSingleton<IHubSpotClientService, HubSpotClientService>();
        services.AddSingleton<IEngineSyncStatusRepository, EngineSyncStatusRepository>();
        services.AddSingleton<IEngineSyncStatusService, EngineSyncStatusService>();
        services.AddSingleton<IHubSpotClientService, HubSpotClientService>();
        services.AddSingleton<IDashDocClientService, DashDocClientService>();
        services.AddSingleton<IDashdocShipperService, DashdocShipperService>();
        services.AddSingleton<IEngineClientParticulierRepository, EngineClientParticulierRepository>();
        services.AddSingleton<IEngineClientParticulierService, EngineClientParticulierService>();
        services.AddSingleton<IEngineParametterRepository, EngineParametterRepository>();
        services.AddSingleton<IEngineParametterService, EngineParametterService>();
        services.AddScoped<ISyncBalanceCron, SyncBalanceCron>();
    }
}
