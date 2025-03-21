// ClientParticulierController.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;
using Swashbuckle.AspNetCore.Annotations;
using ILogger = NLog.ILogger;

namespace RecyOs.Controllers;

[Route("client_particulier")]
public class ClientParticulierController : BaseApiController
{
    private readonly IClientParticulierService _clientParticulierService;
    private readonly ILogger<ClientParticulierController> _logger;
    public ClientParticulierController(
        IClientParticulierService clientParticulierService,
        ILogger<ClientParticulierController> logger)
    {
        _clientParticulierService = clientParticulierService;
        _logger = logger;
    }
    
    /// <summary>
    /// Permet de créer un client particulier
    /// </summary>
    /// <param name="clientParticulier">Le client particulier à créer.</param>
    /// <returns>Retourne le client particulier créé.</returns>
    [SwaggerResponse(200, "Retourne le client particulier créé.", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpPost]
    [Authorize(Policy = "WriteParticulier")]
    public async Task<IActionResult> CreateClientParticulier([FromForm] ClientParticulierDto clientParticulier)
    {
        try
        {
            var createdClient = await _clientParticulierService.CreateClientParticulierAsync(clientParticulier);
            return Ok(createdClient);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogInformation(ex, "Client creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Récupère une liste de clients particuliers filtrée et le compte total correspondant à ce filtre.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la liste de clients.</param>
    /// <returns>Retourne une liste de clients particuliers et le compte total correspondant au filtre.</returns>
    [SwaggerResponse(200, "Retourne une liste de clients particuliers et le compte total correspondant au filtre.", typeof((IEnumerable<ClientParticulierDto>, int)))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpGet]
    [Authorize(Policy = "ReadParticulier")]
    public async Task<IActionResult> GetFilteredListWithCount([FromQuery] ClientParticulierGridFilter filter)
    {
        try
        {
            filter = filter ?? new ClientParticulierGridFilter();
            var clients = await _clientParticulierService.GetFilteredListWithCountAsync(filter);
            return Ok(clients);
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the list of clients in the repository.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }
    
    /// <summary>
    /// Permet d'obtenir un client particulier par son identifiant
    /// </summary>
    /// <param name="id">L'identifiant du client particulier recherché.</param>
    /// <returns>Retourne un client particulier.</returns>
    [SwaggerResponse(200, "Retourne un client particulier.", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpGet("{id:int}")]
    [Authorize(Policy = "ReadParticulier")]
    public async Task<IActionResult> GetClientParticulierById(int id)
    {
        try
        {
            var client = await _clientParticulierService.GetClientParticulierByIdAsync(id);
            return Ok(client);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository.");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }

    /// <summary>
    /// Permet d'obtenir un client particulier par son nom
    /// </summary>
    /// <param name="prenom">Le prénom du client particulier recherché.</param>
    /// <param name="nom">Le nom du client particulier recherché.</param>
    /// <param name="ville">La ville du client particulier recherché</param>
    /// <returns>Retourne un client particulier.</returns>
    [SwaggerResponse(200, "Retourne un client particulier.", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpGet("{prenom}/{nom}/{ville}")]
    [Authorize(Policy = "ReadParticulier")]
    public async Task<IActionResult> GetClientParticulierByNameAndCityAsync(string prenom, string nom, string ville)
    {
        try
        {
            var client = await _clientParticulierService.GetClientParticulierByNameAndCityAsync(prenom, nom, ville);
            return Ok(client);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }

    /// <summary>
    /// Permet d'obtenir un client particulier par son code MKGT
    /// </summary>
    /// <param name="codeMkgt">Le code MKGT du client particulier recherché.</param>
    /// <returns>Retourne un client particulier.</returns>
    [SwaggerResponse(200, "Retourne un client particulier.", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpGet("{codeMkgt}")]
    [Authorize(Policy = "ReadParticulier")]
    public async Task<IActionResult> GetClientParticulierByCodeMkgt(string codeMkgt)
    {
        try
        {
            var client = await _clientParticulierService.GetClientParticulierByCodeMkgtAsync(codeMkgt);
            return Ok(client);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while getting the client in the repository.");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }
    
    /// <summary>
    /// Permet de mettre à jour un client particulier
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à mettre à jour.</param>
    /// <param name="clientParticulierDto">Le client particulier à mettre à jour.</param>
    /// <returns>Retourne le client particulier mis à jour.</returns>
    [SwaggerResponse(200, "Retourne le client particulier mis à jour.", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpPut("{id:int}")]
    [Authorize(Policy = "WriteParticulier")]
    public async Task<IActionResult> UpdateClientParticulier(int id, ClientParticulierDto clientParticulierDto)
    {
        try
        {
            var updatedClient = await _clientParticulierService.UpdateClientParticulierAsync(id, clientParticulierDto);
            return Ok(updatedClient);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "An error occurred while updating the client in the repository.");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }
    
    /// <summary>
    /// Permet de marquer un client particulier comme supprimé sans le retirer physiquement de la base de données (soft delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à marquer comme supprimé.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    [SwaggerResponse(200, "Retourne un booléen indiquant si la suppression a réussi.")]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> SoftDeleteClientParticulier(int id)
    {
        try
        {
            var deleted = await _clientParticulierService.SoftDeleteClientParticulierAsync(id);
            return Ok(deleted);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "The entity was not found in the database.");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }
    
    /// <summary>
    /// Permet de supprimer définitivement un client particulier de la base de données (hard delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à supprimer.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    [SwaggerResponse(200, "Retourne un booléen indiquant si la suppression a réussi.")]
    [SwaggerResponse(400, "Requête invalide.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client particulier non trouvé.")]
    [SwaggerResponse(500, "Erreur interne du serveur.")]
    [HttpDelete("hard/{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> HardDeleteClientParticulier(int id)
    {
        try
        {
            var deleted = await _clientParticulierService.HardDeleteClientParticulierAsync(id);
            return Ok(deleted);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, "The entity was not found in the database.");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred: {Message}", ex.Message);
            return StatusCode(500, new { message = "An unexpected error occurred while processing the request." });
        }
    }

    /// <summary>
    /// Supprime un code ERP d'un client particulier.
    /// </summary>
    /// <param name="id">L'identifiant du client.</param>
    /// <param name="codeType">Le type d'ERP visé</param>
    /// <returns>L'entitée modifiée</returns>
    [SwaggerResponse(200, "Code ERP supprimé", typeof(ClientParticulierDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "ClientParticulier non trouvé")]
    [HttpPut]
    [Route("code-erp/{id:int}/{codeType}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> DeleteErpCodeAsync(int id, string codeType)
    {
        var result = await _clientParticulierService.DeleteErpCodeAsync(id, codeType);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}