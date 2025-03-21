//  BaseModule.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using NLog;

namespace RecyOs.Engine.Modules;

public abstract class BaseModule<TSource, TDestination> : IEngineModule
    where TSource : class
    where TDestination : class
{
    private readonly IDataService<TDestination> _dataService;
    private readonly IHubService<TSource> _hubService;
    private readonly IMapper _mapper;
    private readonly ILogger<BaseModule<TSource, TDestination>> _logger;
    public string ModuleName { get; protected set; }
    protected readonly bool enableCreationSync;
    protected readonly bool enableUpdateSync;
    protected readonly bool idCreatedByDestination;

    protected BaseModule(string moduleName, IDataService<TDestination> dataService, 
        IHubService<TSource> hubService, IMapper mapper, IEngineSyncStatusService engineSyncStatusService,
        ILogger<BaseModule<TSource,TDestination>> logger)
    {
        ModuleName = moduleName;
        _dataService = dataService;
        _hubService = hubService;
        _mapper = mapper;
        var objSyncEngine = engineSyncStatusService.GetByModuleName(moduleName);
        enableCreationSync = objSyncEngine.SyncCre;
        enableUpdateSync = objSyncEngine.SyncUpd;
        idCreatedByDestination = objSyncEngine.IdCreatedByDest ?? false;
        _logger = logger;
    }

    /*
     * Méthode : SyncData
     * 
     * Description :
     * Cette méthode effectue la synchronisation des objets modifiés et créés à partir d'un service Hub et les envoie
     * à un service de données pour les mettre à jour ou les ajouter à la base de données. Elle retourne un booléen
     * pour indiquer si la synchronisation a réussi ou non.
     * 
     * Paramètres : aucun
     * 
     * Retour : bool
     * - true si la synchronisation a réussi pour les objets modifiés et créés
     * - false si la synchronisation a échoué pour l'un ou l'autre des types d'objets
     * 
     * Fonctionnement :
     * 1. Récupère les objets Crées à partir du service Hub.
     * 2. Mappe les objets récupérés dans une liste de la classe de destination.
     * 3. Envoie les objets mappés au service de données pour les ajouter à la base de données.
     * 4. Met a jour les liens dans le hub data (si le module le nécéssite)
     * 5. Récupère les objets créés à partir du service Hub.
     * 6. Mappe les objets récupérés dans une liste de la classe de destination.
     * 7. Envoie les objets mappés au service de données pour les mettre à jour.
     * 8. Retourne le résultat de l'étape 3 ET l'étape 6.
     */
    public bool SyncData()
    {
        List<TDestination> destination;
        List<TSource> retours;
        _logger.LogTrace($"Synchronisation des données pour le module {ModuleName}.");
        if (enableCreationSync)
        {
            _logger.LogTrace($"Synchronisation des fiches crées.");
            // 1. Synchronisation des objets créés
            IList<TSource> source = _hubService.GetCreatedItems(ModuleName);
            destination = new List<TDestination>();
            retours = new List<TSource>();

            foreach (var element in source)
            {
                destination.Add(_mapper.Map<TDestination>(element));
            }

            var items = _dataService.AddItems(destination);
            if (idCreatedByDestination)
            {
                foreach (var retruned in items)
                {
                    retours.Add(_mapper.Map<TSource>(retruned));
                }
                _hubService.CallBackDestIdCreation(ModuleName, retours);
            }
            _logger.LogTrace($"Synchronisation des fiches crées terminée. Nombre de fiches : {destination.Count}.");
        }

        if (enableUpdateSync)
        {
            _logger.LogTrace($"Synchronisation des fiches mises à jour.");
            // 5. Synchronisation des objets modifiés
            IList<TSource> updatedItems = _hubService.GetUpdatedItems(ModuleName);
            destination = new List<TDestination>();

            foreach (var element in updatedItems)
            {
                destination.Add(_mapper.Map<TDestination>(element));
            }

            _logger.LogTrace($"Synchronisation des fiches mises à jour terminée. Nombre de fiches : {updatedItems.Count}");
            // 8. Retourne le résultat de l'étape 3 ET l'étape 6.
            _dataService.UpdateItems(destination);
        }
        _logger.LogTrace($"Synchronisation des données pour le module {ModuleName} terminée.");
        return true;
    }
}