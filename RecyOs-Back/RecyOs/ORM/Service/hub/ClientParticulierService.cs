// ClientParticulierService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.ORM.Service.hub;

/// <summary>
/// Service class for managing ClientParticulier entities.
/// This class provides methods for creating, retrieving, updating, and deleting client records.
/// </summary>
public class ClientParticulierService : BaseService, IClientParticulierService
{
    private readonly IClientParticulierRepository _clientParticulierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientParticulierService> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientParticulierService"/> class.
    /// </summary>
    /// <param name="clientParticulierRepository">The repository to manage client particulier data.</param>
    /// <param name="mapper">The mapper for transforming entities to DTOs and vice versa.</param>
    /// <param name="logger">The logger to log errors and information.</param>
    public ClientParticulierService(
        ICurrentContextProvider contextProvider,
        IClientParticulierRepository clientParticulierRepository, 
        IMapper mapper, 
        ILogger<ClientParticulierService> logger) : base(contextProvider)
    {
        _clientParticulierRepository = clientParticulierRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Creates a new ClientParticulier entity and returns its DTO.
    /// </summary>
    /// <param name="clientParticulierDto">The DTO object to create.</param>
    /// <returns>The created ClientParticulier as a DTO.</returns>
    public async Task<ClientParticulierDto> CreateClientParticulierAsync(ClientParticulierDto clientParticulierDto)
    {
        try
        {
            var normalizedClient = await NormalizeClientFields(clientParticulierDto);
            var client = _mapper.Map<ClientParticulier>(normalizedClient);
            var result = await _clientParticulierRepository.CreateClientParticulierAsync(client, new ContextSession());

            return _mapper.Map<ClientParticulierDto>(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogInformation(ex, "An error occurred while creating the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a filtered list of ClientParticulier entities along with the total count.
    /// </summary>
    /// <param name="filter">The filter criteria for retrieving clients.</param>
    /// <param name="includeDeleted">Whether to include deleted clients in the result.</param>
    /// <returns>A tuple containing the filtered list of clients as DTOs and the total count.</returns>
    public async Task<GridData<ClientParticulierDto>> GetFilteredListWithCountAsync(ClientParticulierGridFilter filter, bool includeDeleted = false)
    {
        try
        {
            var tuple = await _clientParticulierRepository.GetFilteredListWithCountAsync(filter, Session, includeDeleted);
            var ratio = tuple.Item2 / (double)filter.PageSize;
            var begin = filter.PageNumber  * filter.PageSize;
        
            return new GridData<ClientParticulierDto>
            {
                Items = _mapper.Map<IEnumerable<ClientParticulierDto>>(tuple.Item1),
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
        catch (InvalidOperationException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the list of clients in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all instances of <see cref="ClientParticulierDto"/> from the repository.
    /// </summary>
    /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
    /// <returns>An array of <see cref="ClientParticulierDto"/>.</returns>
    public async Task<IList<ClientParticulierDto>> GetAll(bool includeDeleted = false)
    {
        var etablissements = await _clientParticulierRepository.GetAll(Session, includeDeleted);
        return _mapper.Map<IList<ClientParticulierDto>>(etablissements);
    }

    /// <summary>
    /// Retrieves a ClientParticulier by its ID.
    /// </summary>
    /// <param name="id">The ID of the client to retrieve.</param>
    /// <param name="includeDeleted">Whether to include deleted clients in the result.</param>
    /// <returns>The ClientParticulier as a DTO.</returns>
    public async Task<ClientParticulierDto> GetClientParticulierByIdAsync(int id, bool includeDeleted = false)
    {
        try
        {
            var result = await _clientParticulierRepository.GetClientParticulierByIdAsync(id, new ContextSession(), includeDeleted);
            return _mapper.Map<ClientParticulierDto>(result);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="prenom">The client first name.</param>
    /// <param name="nom">The client last name.</param>
    /// <param name="ville">La ville du client particulier recherché</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier DTO or null if not found.</returns>
    public async Task<ClientParticulierDto> GetClientParticulierByNameAndCityAsync(string prenom, string nom, string ville, bool includeDeleted = false)
    {
        try
        {
            var result = await _clientParticulierRepository.GetClientParticulierByNameAndCityAsync(prenom, nom, ville, new ContextSession(), includeDeleted);
            return _mapper.Map<ClientParticulierDto>(result);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="codeMkgt">The client MKGT code.</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier DTO or null if not found.</returns>
    public async Task<ClientParticulierDto> GetClientParticulierByCodeMkgtAsync(string codeMkgt, bool includeDeleted = false)
    {
        try
        {
            var result = await _clientParticulierRepository.GetClientParticulierByCodeMkgtAsync(codeMkgt, new ContextSession(), includeDeleted);
            return _mapper.Map<ClientParticulierDto>(result);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing ClientParticulier.
    /// </summary>
    /// <param name="id">The ID of the client to update.</param>
    /// <param name="clientParticulierDto">The updated client data.</param>
    /// <returns>The updated ClientParticulier as a DTO.</returns>
    public async Task<ClientParticulierDto> UpdateClientParticulierAsync(int id, ClientParticulierDto clientParticulierDto)
    {
        try
        {
            var normalizedClient = await NormalizeClientFields(clientParticulierDto);
            var client = _mapper.Map<ClientParticulier>(normalizedClient);

            var result = await _clientParticulierRepository.UpdateClientParticulierAsync(id, client, new ContextSession());
            return _mapper.Map<ClientParticulierDto>(result);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while updating the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Soft deletes a ClientParticulier by marking it as deleted without removing it from the database.
    /// </summary>
    /// <param name="id">The ID of the client to soft delete.</param>
    /// <returns>A boolean indicating whether the operation was successful.</returns>
    public async Task<bool> SoftDeleteClientParticulierAsync(int id)
    {
        try
        {
            return await _clientParticulierRepository.SoftDeleteClientParticulierAsync(id, new ContextSession());
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while soft deleting the client in the repository.");
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
            throw;
        }
    }

    /// <summary>
    /// Hard deletes a ClientParticulier by removing it from the database.
    /// </summary>
    /// <param name="id">The ID of the client to hard delete.</param>
    /// <returns>A boolean indicating whether the operation was successful.</returns>
    public async Task<bool> HardDeleteClientParticulierAsync(int id)
    {
        try
        {
            return await _clientParticulierRepository.HardDeleteClientParticulierAsync(id, new ContextSession());
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
            throw;
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
            throw;
        }
    }

    private static async Task<ClientParticulierDto> NormalizeClientFields(ClientParticulierDto clientDto)
    {
        clientDto.Nom = clientDto.Nom.ToUpper();
        clientDto.Prenom = char.ToUpper(clientDto.Prenom[0]) + clientDto.Prenom.Substring(1).ToLower();
        clientDto.AdresseFacturation1 = StringNormalizerHelper.RemoveAccentsAndUppercase(clientDto.AdresseFacturation1);
        clientDto.AdresseFacturation2 = StringNormalizerHelper.RemoveAccentsAndUppercase(clientDto.AdresseFacturation2);
        clientDto.AdresseFacturation3 = StringNormalizerHelper.RemoveAccentsAndUppercase(clientDto.AdresseFacturation3);
        clientDto.PaysFacturation = StringNormalizerHelper.RemoveAccentsAndUppercase(clientDto.PaysFacturation);
        clientDto.VilleFacturation = StringNormalizerHelper.RemoveAccentsAndUppercase((clientDto.VilleFacturation));
        clientDto.TelephoneFacturation = PhoneNumberHelper.FormatPhoneNumber(clientDto.PaysFacturation, clientDto.TelephoneFacturation);
        clientDto.PortableFacturation = PhoneNumberHelper.FormatPhoneNumber(clientDto.PaysFacturation, clientDto.PortableFacturation);
        clientDto.TelephoneAlternatif = PhoneNumberHelper.FormatPhoneNumber(clientDto.PaysFacturation, clientDto.TelephoneAlternatif);
        clientDto.PortableAlternatif = PhoneNumberHelper.FormatPhoneNumber(clientDto.PaysFacturation, clientDto.PortableAlternatif);

        return clientDto;
    }

    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI..."
    /// </summary>
    /// <param name="id">L'ID du client à mettre à jour.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne le client mis à jour.</returns>
    public async Task<ClientParticulierDto> DeleteErpCodeAsync(int id, string codeType)
    {
        var etablissement = await _clientParticulierRepository.DeleteErpCodeAsync(id, codeType, Session);
        return _mapper.Map<ClientParticulierDto>(etablissement);
    }
}