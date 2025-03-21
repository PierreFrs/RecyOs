using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class EuropeService<TClientEurope, TRepository> : BaseService
    where TClientEurope : ClientEurope where TRepository : IEuropeRepository<TClientEurope>
{
    protected readonly TRepository _etablissementRepository;
    protected readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    
    protected EuropeService(ICurrentContextProvider contextProvider,
        TRepository etablissementClientRepository, IGroupRepository groupRepository, IMapper mapper)
        : base(contextProvider)
    {
        _etablissementRepository = etablissementClientRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Récupère les données filtrées pour une grille clients européens.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un objet contenant les éléments clients correspondant au filtrage et les informations de pagination.</returns>
    public async Task<GridData<ClientEuropeDto>> GetGridDataAsync(ClientEuropeGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _etablissementRepository.GetFiltredListWithCount(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<ClientEuropeDto>
        {
            Items = _mapper.Map<IEnumerable<ClientEuropeDto>>(tuple.Item1),
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
    /// Récupère un client Européen par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant à l'identifiant donné.</returns>
    public async Task<ClientEuropeDto> GetById(int id, bool includeDeleted = false)
    {
        var clientEurope = await _etablissementRepository.GetById(id, Session, includeDeleted);
        return _mapper.Map<ClientEuropeDto>(clientEurope);
    }
    
    /// <summary>
    /// Récupère un client Européen par son code TVA.
    /// </summary>
    /// <param name="vat">Le code TVA du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code TVA donné.</returns>
    public async Task<ClientEuropeDto> GetByVat(string vat, bool includeDeleted = false)
    {
        var clientEurope = await _etablissementRepository.GetByVat(vat, Session, includeDeleted);
        return _mapper.Map<ClientEuropeDto>(clientEurope);
    }
    
    /// <summary>
    /// Récupère un client Européen par son code Kerlog.
    /// </summary>
    /// <param name="codeKerlog">Le code Kerlog du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code Kerlog donné.</returns>
    public async Task<ClientEuropeDto> GetByCodeKerlog(string codeKerlog, bool includeDeleted = false)
    {
        var clientEurope = await _etablissementRepository.GetByCodeKerlog(codeKerlog, Session, includeDeleted);
        return _mapper.Map<ClientEuropeDto>(clientEurope);
    }
    
    /// <summary>
    /// Récupère un client Européen par son code MKGT.
    /// </summary>
    /// <param name="codeMkgt">Le code MKGT du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code MKGT donné.</returns>
    public async Task<ClientEuropeDto> GetByCodeMkgt(string codeMkgt, bool includeDeleted = false)
    {
        var clientEurope = await _etablissementRepository.GetByCodeMkgt(codeMkgt, Session, includeDeleted);
        return _mapper.Map<ClientEuropeDto>(clientEurope);
    }
    
    /// <summary>
    /// Récupère un client Européen par son ID Odoo.
    /// </summary>
    /// <param name="idOdoo">L'ID Odoo du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant à l'ID Odoo donné.</returns>
    public async Task<ClientEuropeDto> GetByIdOdoo(string idOdoo, bool includeDeleted = false)
    {
        var clientEurope = await _etablissementRepository.GetByIdOdoo(idOdoo, Session, includeDeleted);
        return _mapper.Map<ClientEuropeDto>(clientEurope);
    }

    /// <summary>
    /// Récupère le groupe d'un client européen par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen.</param>
    /// <returns>Retourne le groupe du client européen.</returns>
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
    /// Modifie un client Européen à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations à jour du client européen.</param>
    /// <returns>Retourne le client européen mis à jour.</returns>
    /// <remarks>Le numéro de TVA ne peut pas être modifié.</remarks>
    public async Task<ClientEuropeDto> Update(ClientEuropeDto dto)
    {
        var clientEurope = _mapper.Map<TClientEurope>(dto);
        try
        {
            await _etablissementRepository.UpdateAsync(clientEurope, Session);
        }
        catch (DbUpdateException e)
        {
            throw new Exception("Erreur lors de la mise à jour du client européen.", e);
        }
        return _mapper.Map<ClientEuropeDto>(clientEurope);
        
    }

    /// <summary>
    /// Crée un nouveau client Européen à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations du client européen à créer.</param>
    /// <param name="disableTracking"></param>
    /// <returns>Retourne le client européen créé.</returns>
    public async Task<ClientEuropeDto> Create(ClientEuropeDto dto)
    {
        var clientEurope = _mapper.Map<TClientEurope>(dto);
        var res = await _etablissementRepository.CreateAsync(clientEurope, Session);
        return _mapper.Map<ClientEuropeDto>(res);
    }

    /// <summary>
    /// Retrieves all instances of <see cref="ClientEuropeDto"/> from the repository.
    /// </summary>
    /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
    /// <returns>An array of <see cref="ClientEuropeDto"/>.</returns>
    public async Task<IList<ClientEuropeDto>> GetAll(bool includeDeleted = false)
    {
        var etablissements = await _etablissementRepository.GetAll(Session, includeDeleted);
        return _mapper.Map<IList<ClientEuropeDto>>(etablissements);
    }

    /// <summary>
    /// Crée un nouvel établissement contenant uniquement un code tva
    /// </summary>
    /// <param name="vat">L'identifiant administratif de l'établissement</param>
    /// <returns>Le DTO de l'établissement créé</returns>
    public async Task<ClientEuropeDto> CreateFromScratchAsync(string vat)
    {
        var clientEuropeDto = new ClientEuropeDto { Vat = vat };
        var returnedClient = await Create(clientEuropeDto);
        if (returnedClient == null)
        {
            throw new Exception("Impossible de créer l'établissement.");
        }
        return _mapper.Map<ClientEuropeDto>(returnedClient);
    }
    
    /// <summary>
    /// Supprime un client Européen en fonction de son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen à supprimer.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    public async Task<bool> DeleteAsync(int id, bool estClient, bool estFournisseur)
    {
        // Retrieve the entity
        var clientEurope = await _etablissementRepository.GetById(id, Session);

        if (clientEurope == null)
        {
            // Entity does not exist
            return false;
        }

        bool entityModified = false;

        // Update 'Client' status if requested
        if (estClient && clientEurope.Client)
        {
            clientEurope.Client = false;
            entityModified = true;
        }

        // Update 'Fournisseur' status if requested
        if (estFournisseur && clientEurope.Fournisseur)
        {
            clientEurope.Fournisseur = false;
            entityModified = true;
        }

        // If the entity was modified, decide whether to update or delete
        if (entityModified)
        {
            if (!clientEurope.Client && !clientEurope.Fournisseur)
            {
                // Both entities are false; update the entity
                await _etablissementRepository.UpdateAsync(clientEurope, Session);

                // Both statuses are false; delete the entity
                await _etablissementRepository.Delete(id, Session);
            }
            else
            {
                // Update the entity
                await _etablissementRepository.UpdateAsync(clientEurope, Session);
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
    /// Supprime les codes relatifs aux ERP (MKGT, GPI..."
    /// </summary>
    /// <param name="etablissementClient">L'établissement client européen à mettre à jour.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne l'établissement client européen mis à jour.</returns>
    public async Task<ClientEuropeDto> DeleteErpCodeAsync(int id, string codeType)
    {
        var etablissement = await _etablissementRepository.DeleteErpCodeAsync(id, codeType, Session);
        return _mapper.Map<ClientEuropeDto>(etablissement);
    }
}