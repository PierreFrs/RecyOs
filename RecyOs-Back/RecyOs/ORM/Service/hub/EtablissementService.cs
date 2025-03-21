using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;
using RecyOs.Helpers;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Results;
using ILogger = NLog.ILogger;

namespace RecyOs.ORM.Service.hub;

public abstract class EtablissementService<TEtablissementClient, TRepository> : BaseService
    where TEtablissementClient : EtablissementClient, new() where TRepository : IEtablissementRepository<TEtablissementClient>
{
    protected readonly TRepository _etablissementRepository;
    protected readonly IEtablissementFicheRepository<EtablissementFiche>  _etablissementFicheRepository;
    protected readonly IEntrepriseBaseRepository<EntrepriseBase> _entrepriseBaseRepository;
    private readonly IPappersUtilitiesService _pappersUtilitiesService;
    private readonly IEtablissementServiceUtilitaryMethods _etablissementServiceUtilitaryMethods;
    private readonly ITokenInfoService _tokenInfoService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IGroupRepository _groupRepository;

    protected EtablissementService(
        ICurrentContextProvider contextProvider,
        TRepository etablissementClientRepository,
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository,
        IFCliRepository<Fcli> fcliRepository,
        IPappersUtilitiesService pappersUtilitiesService,
        IEtablissementServiceUtilitaryMethods etablissementServiceUtilitaryMethods,
        ITokenInfoService tokenInfoService,
        IMapper mapper,
        IGroupRepository groupRepository)
        : base(contextProvider)
    {
        _logger.Trace("Initialisation du service EtablissementClient");
        _etablissementRepository = etablissementClientRepository;
        _etablissementFicheRepository = etablissementFicheRepository;
        _pappersUtilitiesService = pappersUtilitiesService;
        _etablissementServiceUtilitaryMethods = etablissementServiceUtilitaryMethods;
        _tokenInfoService = tokenInfoService;
        _mapper = mapper;
        _groupRepository = groupRepository;
    }
    
    /// <summary>
    /// Récupère les données filtrées pour une grille d'établissements clients.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un objet contenant les éléments de l'établissement client correspondant au filtrage et les informations de pagination.</returns>
    public async Task<GridData<EtablissementClientDto>> GetDataForGrid(EtablissementClientGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _etablissementRepository.GetFiltredListWithCount(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<EtablissementClientDto>
        {
            Items = _mapper.Map<IEnumerable<EtablissementClientDto>>(tuple.Item1),
            Paginator = new Pagination()
            {
                length = tuple.Item2,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio),1.0),
                startIndex = begin,
            }
        };
    }
    
    /// <summary>
    /// Récupère un établissement client par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant à l'identifiant donné.</returns>
    public async Task<EtablissementClientDto> GetById(int id, bool includeDeleted = false)
    {
        var etablissementClient = await _etablissementRepository.GetById(id, Session, includeDeleted);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }
    
    /// <summary>
    /// Récupère un établissement client par son numéro SIRET.
    /// </summary>
    /// <param name="siret">Le numéro SIRET de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au numéro SIRET donné.</returns>
    public async Task<EtablissementClientDto> GetBySiret(string siret, bool includeDeleted = false)
    {
        var etablissementClient = await _etablissementRepository.GetBySiret(siret, Session, includeDeleted);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }
    
    /// <summary>
    /// Récupère un établissement client par son code Kerlog.
    /// </summary>
    /// <param name="codeKerlog">Le code Kerlog de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au code Kerlog donné.</returns>
    public async Task<EtablissementClientDto> GetByCodeKerlog(string codeKerlog, bool includeDeleted = false)
    {
        var etablissementClient = await _etablissementRepository.GetByCodeKerlog(codeKerlog, Session, includeDeleted);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }
    
    /// <summary>
    /// Récupère un établissement client par son code MKGT.
    /// </summary>
    /// <param name="codeMkgt">Le code MKGT de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au code MKGT donné.</returns>
    public async Task<EtablissementClientDto> GetByCodeMkgt(string codeMkgt, bool includeDeleted = false)
    {
        var etablissementClient = await _etablissementRepository.GetByCodeMkgt(codeMkgt, Session, includeDeleted);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }
    
    /// <summary>
    /// Récupère un établissement client par son ID Odoo.
    /// </summary>
    /// <param name="idOdoo">L'ID Odoo de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant à l'ID Odoo donné.</returns>
    public async Task<EtablissementClientDto> GetByIdOdoo(string idOdoo, bool includeDeleted = false)
    {
        var etablissementClient = await _etablissementRepository.GetByIdOdoo(idOdoo, Session, includeDeleted);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }

    /// <summary>
    /// Récupère le groupe associé à un établissement client.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <returns>Retourne le groupe associé à l'établissement client.</returns>
    public async Task<GroupDto> GetGroup(int id)
    {
        var client = await _etablissementRepository.GetById(id, Session);
        if (client.GroupId.HasValue) 
        {
            var group = await _groupRepository.GetByIdAsync(client.GroupId.Value, Session);
            return _mapper.Map<GroupDto>(group);
        }
        return null;
    }

    /// <summary>
    /// Récupère une liste d'établissement partageant un même SIREN.
    /// </summary>
    /// <param name="siren">Le SIREN des établissements recherchés.</param>
    /// <returns>Retourne une liste de DTO des établissements partageant le même SIREN.</returns>
    public async Task<IEnumerable<EtablissementClientDto>> GetEtablissementGroupBySirenAsync(string siren)
    {
        IEnumerable<TEtablissementClient> etablissements = await _etablissementRepository.GetEtablissementGroupBySirenAsync(siren, Session);
        return _mapper.Map<IList<EtablissementClientDto>>(etablissements);
    }
    
    /// <summary>
    /// Supprime un établissement
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement à supprimer</param>
    /// <param name="estClient">Indique si c'est un client</param>
    /// <param name="estFournisseur">Indique si c'est un fournisseur</param>
    /// <returns>True si la suppression a réussi</returns>
    public async Task<bool> DeleteAsync(int id, bool estClient, bool estFournisseur)
    {
        // Retrieve the entity
        var etablissementClient = await _etablissementRepository.GetById(id, Session);

        if (etablissementClient == null)
        {
            // Entity does not exist
            return false;
        }

        bool entityModified = false;

        // Update 'Client' status if requested
        if (estClient && etablissementClient.Client)
        {
            etablissementClient.Client = false;
            entityModified = true;
        }

        // Update 'Fournisseur' status if requested
        if (estFournisseur && etablissementClient.Fournisseur)
        {
            etablissementClient.Fournisseur = false;
            entityModified = true;
        }

        // If the entity was modified, decide whether to update or delete
        if (entityModified)
        {
            if (!etablissementClient.Client && !etablissementClient.Fournisseur)
            {
                // Both entities are false; update the entity
                await _etablissementRepository.UpdateAsync(etablissementClient, Session);

                // Both statuses are false; delete the entity
                await _etablissementRepository.Delete(id, Session);

                // Delete the associated 'Fiche' if it exists
                var fiche = await _etablissementFicheRepository.GetBySiret(etablissementClient.Siret, Session);
                if (fiche != null)
                {
                    await _etablissementFicheRepository.Delete(fiche.Id, Session);
                }
            }
            else
            {
                // Update the entity
                await _etablissementRepository.UpdateAsync(etablissementClient, Session);
            }

            return true;
        }
        else
        {
            // No changes were made; nothing to delete or update
            return false;
        }
    }
    
    /// <summary>
    /// Modifie un établissement client à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations à jour de l'établissement client.</param>
    /// <returns>Retourne un DTO de l'établissement client après modification.</returns>
    /// <exception cref="Exception">Lève une exception en cas d'échec de la mise à jour en base de données.</exception>
    public async Task<EtablissementClientDto> Edit(EtablissementClientDto dto)
    {
        _logger.Info($"Mise à jour de l'établissement client {dto.Id} par {_tokenInfoService.GetCurrentUserName()}");
        var etablissementClient = _mapper.Map<TEtablissementClient>(dto);
        try
        {
            await _etablissementRepository.UpdateAsync(etablissementClient, Session);
        }
        catch (DbUpdateException e)
        {
            _logger.Error(e, $"Impossible de mettre à jour l'établissement client {etablissementClient.Id}");
            throw new Exception("Impossible de mettre à jour l'établissement client", e);
        }
        
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }

    /// <summary>
    /// Modifie le Siret d'un établissement client à partir d'une string fourni.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <param name="newSiret">Le siret de l'établissement client</param>
    /// <param name="estClient"></param>
    /// <param name="estFournisseur"></param>
    /// <param name="session"></param>
    /// <returns>Retourne un DTO de l'établissement client après modification.</returns>
    public async Task<ServiceResult> ChangeSiretAsync(int id, string newSiret, bool estClient, bool estFournisseur, ContextSession session)
    {
        _logger.Info($"Changement de SIRET pour l'établissement client {id} en {newSiret} client {estClient} fournisseur {estFournisseur} par {_tokenInfoService.GetCurrentUserName()}");
        // Check if the new SIRET is valid
        _logger.Trace($"Vérification de la validité du nouveau SIRET {newSiret} pour l'établissement client {id} par {_tokenInfoService.GetCurrentUserName()}");
        var validationResult = await _etablissementServiceUtilitaryMethods.EntityChecks(id, newSiret, session);
        if (!validationResult.Success)
        {
            _logger.Error($"Erreur lors du changement de SIRET pour l'établissement client {id} en {newSiret} par {_tokenInfoService.GetCurrentUserName()}");
            return validationResult;
        }

        // Fetch the old EtablissementClient and EtablissementFiche
        _logger.Trace($"Récupération de l'établissement client {id} et de la fiche correspondante par {_tokenInfoService.GetCurrentUserName()}");
        var oldEtablissementClient = await _etablissementRepository.GetById(id, session);
        if (oldEtablissementClient == null)
        {
            _logger.Error($"L'établissement client {id} n'a pas été trouvé.");
            return new ServiceResult
            {
                Success = false,
                Message = "L'établissement client n'a pas été trouvé."
            };
        }

        _logger.Trace($"Récupération de la fiche de l'établissement client {id} par {_tokenInfoService.GetCurrentUserName()}");
        var oldEtablissementFiche = await _etablissementFicheRepository.GetBySiret(oldEtablissementClient.Siret, session);
        if (oldEtablissementFiche == null)
        {
            _logger.Error($"La fiche de l'établissement client {id} n'a pas été trouvée.");
            return new ServiceResult
            {
                Success = false,
                Message = "La fiche n'a pas été trouvée."
            };
        }
        
        // Create a new etablissement with the new siret and auto complete the original infos
        var newEtablissementClient = _mapper.Map<TEtablissementClient>(await _pappersUtilitiesService.CreateEtablissementClientBySiret(newSiret, estClient, estFournisseur));
        _mapper.Map(oldEtablissementClient, newEtablissementClient);
        newEtablissementClient.UpdatedAt = DateTime.Now;
        newEtablissementClient.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        
        // Delete CodeMkgt in old entity
        await _etablissementRepository.DeleteErpCodeAsync(oldEtablissementClient.Id, "mkgt", session);
        _logger.Trace($"Suppression du code MKGT pour l'établissement client {oldEtablissementClient.Id} par {_tokenInfoService.GetCurrentUserName()}");

        // Save the updated entities back to the database
        _logger.Trace($"Mise à jour de l'établissement client {oldEtablissementClient.Id} avec le nouveau SIRET {newSiret} par {_tokenInfoService.GetCurrentUserName()}");
        await _etablissementRepository.UpdateWithNewSiretAsync(newEtablissementClient, session);

        // Delete old EtablissementClient and Fiche
        _logger.Trace($"Suppression de l'ancien établissement client {oldEtablissementClient.Id} et de la fiche {oldEtablissementFiche.Id} par {_tokenInfoService.GetCurrentUserName()}");
        await _etablissementRepository.Delete(id, session);
        _logger.Trace($"Suppression de l'ancienne fiche {oldEtablissementFiche.Id} par {_tokenInfoService.GetCurrentUserName()}");
        await _etablissementFicheRepository.Delete(oldEtablissementFiche.Id, session);
        
        var dependantEntitiesUpdateResponse = await _etablissementServiceUtilitaryMethods.UpdateIdsInDependantEntities(oldEtablissementClient.Id, newEtablissementClient.Id, session);
        if (!dependantEntitiesUpdateResponse.Success)
        {
            _logger.Error($"Erreur lors de la mise à jour des entités dépendantes de l'établissement client {oldEtablissementClient.Id} en {newEtablissementClient.Id} par {_tokenInfoService.GetCurrentUserName()}");
            return dependantEntitiesUpdateResponse;
        }
        // Verify the deletion
        _logger.Trace($"Vérification de la suppression de l'ancien établissement client {oldEtablissementClient.Id} et de la fiche {oldEtablissementFiche.Id} par {_tokenInfoService.GetCurrentUserName()}");
        var checkEtablissementClient = await _etablissementRepository.GetBySiret(oldEtablissementClient.Siret, session);
        var checkEtablissementFiche = await _etablissementFicheRepository.GetBySiret(oldEtablissementFiche.Siret, session);

        if (checkEtablissementClient != null || checkEtablissementFiche != null)
        {
            _logger.Error($"L'ancien établissement client {oldEtablissementClient.Id} ou la fiche {oldEtablissementFiche.Id} n'a pas été supprimé.");
            return new ServiceResult
            {
                Success = false,
                Message = "L'ancien établissement client ou la fiche n'a pas été supprimé."
            };
        }

        _logger.Info($"Changement de SIRET pour l'établissement client {id} en {newSiret} client {estClient} fournisseur {estFournisseur} par {_tokenInfoService.GetCurrentUserName()} réussi.");
        return new ServiceResult
        {
            Success = true,
            StatusCode = 200,
        };
    }
    
    /// <summary>
    /// Crée un nouvel établissement client à partir d'un DTO et l'associe à une entreprise et à une fiche d'établissement, également fournies sous forme de DTO.
    /// </summary>
    /// <param name="dto">Le DTO représentant l'établissement client à créer.</param>
    /// <param name="entrepriseBaseDto">Le DTO représentant l'entreprise de base associée à l'établissement client.</param>
    /// <param name="etablissementFicheDto">Le DTO représentant la fiche d'établissement associée à l'établissement client.</param>
    /// <param name="disableTracking"></param>
    /// <returns>Retourne un DTO de l'établissement client créé.</returns>
    public async Task<EtablissementClientDto> Create(EtablissementClientDto dto, EntrepriseBaseDto entrepriseBaseDto,
        EtablissementFicheDto etablissementFicheDto, bool disableTracking)
    {
        _logger.Info($"Création d'un nouvel établissement client par {_tokenInfoService.GetCurrentUserName()}");
        var etablissementClient = _mapper.Map<TEtablissementClient>(dto);
        var entrepriseBase = _mapper.Map<EntrepriseBase>(entrepriseBaseDto);
        var etablissementFiche = _mapper.Map<EtablissementFiche>(etablissementFicheDto);
        var res = await _etablissementRepository.Create(etablissementClient, entrepriseBase, etablissementFiche, Session);
        return _mapper.Map<EtablissementClientDto>(res);
    }
    
    /// <summary>
    /// Crée un nouvel établissement client dans la base de données à partir d'un DTO.
    /// Si l'établissement client existe déjà, il ne sera ni créé, ni mis à jour.
    /// </summary>
    /// <param name="dto">Le Data Transfer Object représentant l'établissement client.</param>
    /// <returns>Le DTO de l'établissement client créé.</returns>
    public async Task<EtablissementClientDto> Create(EtablissementClientDto dto)
    {
        _logger.Info($"Création d'un nouvel établissement client par {_tokenInfoService.GetCurrentUserName()}");
        var etablissementClient = _mapper.Map<TEtablissementClient>(dto);
        await _etablissementRepository.CreateIfDoesntExistAsync(etablissementClient, Session);
        return _mapper.Map<EtablissementClientDto>(etablissementClient);
    }

    /// <summary>
    /// Retrieves all instances of <see cref="EtablissementClientDto"/> from the repository.
    /// </summary>
    /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
    /// <returns>An array of <see cref="EtablissementClientDto"/>.</returns>
    public async Task<IList<EtablissementClientDto>> GetAll(bool includeDeleted = false)
    {
        var etablissements = await _etablissementRepository.GetAll(Session, includeDeleted);
        return _mapper.Map<IList<EtablissementClientDto>>(etablissements);
    }

    /// <summary>
    /// Crée un nouvel établissement contenant uniquement un siret
    /// </summary>
    /// <param name="siret">L'identifiant administratif de l'établissement</param>
    /// <returns>Le DTO de l'établissement créé</returns>
    public async Task<EtablissementClientDto> CreateFromScratchAsync(string siret)
    {
        var siren = siret.Substring(0, 9);
        var entrepriseBaseDto = new EntrepriseBaseDto { Siren = siren };
        var etablissementFicheDto = new EtablissementFicheDto { Siret = siret };
        var etablissementClientDto = new EtablissementClientDto
        {
            Siret = siret,
            Siren = siren
        };
        var returnedClient = await Create(etablissementClientDto, entrepriseBaseDto, etablissementFicheDto, false);

        if (returnedClient == null)
        {
            _logger.Error($"Impossible de créer l'établissement client pour le siret {siret}");
            throw new Exception("Impossible de créer l'établissement.");
        }

        return _mapper.Map<EtablissementClientDto>(returnedClient);
    }

    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI..."
    /// </summary>
    /// <param name="id">L'ID de l'établissement client à mettre à jour.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne l'établissement client mis à jour.</returns>
    public async Task<EtablissementClientDto> DeleteErpCodeAsync(int id, string codeType)
    {
        var etablissement = await _etablissementRepository.DeleteErpCodeAsync(id, codeType, Session);
        return _mapper.Map<EtablissementClientDto>(etablissement);
    }
}
