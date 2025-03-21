//  EngineService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules;
using NLog;
using RecyOs.Engine.Services;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.Engine;

public class SynchroService: BackgroundService
{
    public List<IEngineModule> engineModules;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISynchroWaitingToken _waitingToken;
    private readonly IEngineSyncStatusService _engineSyncStatusService;
    private readonly IMigrationStatusService _migrationStatusService;
    private bool initialized = false;
    
    public SynchroService(IConfiguration config, IServiceProvider prmServiceProvider, ISynchroWaitingToken waitingToken,
        IEngineSyncStatusService syncService, IMigrationStatusService migrationStatusService)
    {
        _serviceProvider = prmServiceProvider;
        engineModules = new List<IEngineModule>();
        _config = config;
        _waitingToken = waitingToken;
        _engineSyncStatusService = syncService;
        _migrationStatusService = migrationStatusService;
    }
    
    public void AddModule(IEngineModule module)
    {
        // si le module est null, on ne l'ajoute pas et déclenche une erreur
        if (module == null)
        {
            _logger.Error("Le module est null");
            throw new ArgumentNullException(nameof(module));
        }
        engineModules.Add(module);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bool isEnabled = _config.GetValue<bool>("engine:enabled");
        int delay = _config.GetValue<int>("engine:delay");
        
        // Attendre que les migrations soient terminées
        while (!_migrationStatusService.IsMigrationCompleted)
        {
            _logger.Info("Waiting for migrations to complete...");
            await Task.Delay(5000, stoppingToken); // Attendre 5 secondes avant de réessayer
        }
        
        if(!initialized)
        {
            initModules();
            initialized = true;
        }

        while (!stoppingToken.IsCancellationRequested && isEnabled)
        {
            _logger.Info("Début de la synchro des données");
            // pour chaque module
            foreach (var module in engineModules)
            {
                // Ratrapage des erreurs déclenchées par le module (pour éviter de stopper la tâche et éviter de rendre indisponnible l'application) Log de l'erreur
                try
                {
                    // Exécuter la synchro des données
                    module.SyncData();
                }
                catch (Exception e)
                {
                    // Log de l'erreur 
                    _logger.Error($"Erreur lors de la synchro des données du module {module.ModuleName} : {e.Message}");
                }
                
            }
            _logger.Info("Fin de la synchro des données");
            try
            {
                // Attendre 15 secondes avant de relancer la synchro suivante
                await Task.Delay(TimeSpan.FromSeconds(delay), _waitingToken.GetCancellationToken());
            }
            catch (Exception e)
            {
                if (e is TaskCanceledException)
                {
                    _logger.Info("Demnde de synchronisation immédiate reçue");
                    _waitingToken.ResetWaitingToken();
                }
                else
                {
                    _logger.Error($"Erreur lors de la synchro des données : {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Initialise les modules du moteur en lisant la liste des modules actifs à partir de la configuration.
    /// Instancie chaque module qui implémente l'interface IEngineModule et ajoute l'instance à la liste des modules moteur.
    /// Enregistre un message d'erreur si l'instanciation d'un module échoue et un avertissement si un module ne correspond pas à l'interface IEngineModule.
    /// </summary>
    public void initModules()
    {
        // Attendre que les migrations soient terminées
        while (!_migrationStatusService.IsMigrationCompleted)
        {
            _logger.Info("Waiting for migrations to complete...");
            Task.Delay(5000); // Attendre 5 secondes avant de réessayer
        }
        IList<EngineSyncStatusDto> modules = _engineSyncStatusService.GetEnabledModules();
        List<string> moduleNames = new List<string>();
        Assembly moduleAssembly = Assembly.Load("RecyOs");

        if (modules != null)
        {
            foreach (var module in modules)
            {
                moduleNames.Add(module.ModuleName);
            }
        }

        if (moduleNames != null)
        {

            foreach (string moduleName in moduleNames)
            {
                Type type = moduleAssembly.GetType("RecyOs.Engine.Modules." + moduleName);
                if ((type != null) && typeof(IEngineModule).IsAssignableFrom(type))
                {
                    try
                    {
                        IEngineModule module = (IEngineModule)ActivatorUtilities.CreateInstance(_serviceProvider, type);
                        engineModules.Add(module);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Erreur lors de la création de l'instance du module {moduleName}: {ex.Message}");
                    }
                }
                else
                {
                    _logger.Warn($" module {moduleName}: Le module n'existe pas ou n'hérite pas de IEngineModule");
                }
            }
        }
    }
    
    /// <summary>
    /// For testing purposes, bypasses the initialization of modules. Do not use in RecyOs project.
    /// </summary>
    public void bypassInitModules()
    {
        initialized = true;
    }
}