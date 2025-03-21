using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.vatlayer;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("client_europe")]
public class ClientEuropeController : BaseApiController
{
    private readonly IClientEuropeService _clientEuropeService;
    private readonly ISynchroWaitingToken _engineWaitingToken;
    private readonly IVatlayerUtilitiesService _vatlayerUtilitiesService;
    private readonly IConfiguration _config;
    
    public ClientEuropeController(
        IClientEuropeService clientEuropeService, 
        ISynchroWaitingToken synchroService,
        IVatlayerUtilitiesService vatlayerUtilitiesService,
        IConfiguration config)
    {
        _clientEuropeService = clientEuropeService;
        _engineWaitingToken = synchroService;
        _vatlayerUtilitiesService = vatlayerUtilitiesService;
        _config = config;
    }
    
    /// <summary>
    /// Permet d'obtenir un client européen par son identifiant
    /// </summary>
    /// <param name="id">L'identifiant du client européen recherché.</param>
    /// <returns>Retourne un client européen.</returns>
    [SwaggerResponse(200, "Retourne un client européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpGet("{id:int}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> Get(int id)
    {
        var clientEurope = await _clientEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des clients européens
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <returns>Retourne une liste de clients européens.</returns>
    [SwaggerResponse(200, "Retourne une liste de clients européens.", typeof(IEnumerable<ClientEuropeDto>))]
    [SwaggerResponse(401, "Non autorisé.")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> Get([FromQuery] ClientEuropeGridFilter filter)
    {
        filter = filter ?? new ClientEuropeGridFilter();
        var clientsEurope = await _clientEuropeService.GetGridDataAsync(filter);
        return Ok(clientsEurope);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet ClientEurope par son code TVA
    /// </summary>
    /// <param name="vat">Le code TVA du client européen recherché.</param>
    /// <returns>Retourne un client européen.</returns>
    [SwaggerResponse(200, "Retourne un client européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpGet("vat/{vat}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByVat(string vat)
    {
        var clientEurope = await _clientEuropeService.GetByVat(vat);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet ClientEurope par son id odoo
    /// </summary>
    /// <param name="idOdoo">Identifiant odoo du client</param>
    /// <returns>Retourne un client européen.</returns>
    [SwaggerResponse(200, "Retourne un client européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpGet("idOdoo/{idOdoo}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByIdOdoo(string idOdoo)
    {
        var clientEurope = await _clientEuropeService.GetByIdOdoo(idOdoo);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// permet d'obtenir un objet ClientEurope par son code mkgt
    /// </summary>
    /// <param name="codeMkgt">Code client Mkgt</param>
    /// <returns>Retourne un client Européen</returns>
    [SwaggerResponse(200, "Retourne un client européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpGet("codeMkgt/{codeMkgt}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByCodeMkgt(string codeMkgt)
    {
        var clientEurope = await _clientEuropeService.GetByCodeMkgt(codeMkgt);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet ClientEurope par son code kerlog
    /// </summary>
    /// <param name="codeKerlog">Code client de l'établissement recherché</param>
    /// <returns>Retourne un client Européen</returns>
    [SwaggerResponse(200, "Retourne un client européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpGet("codeMkgt/{codeKerlog}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByCodeKerlog(string codeKerlog)
    {
        var clientEurope = await _clientEuropeService.GetByCodeKerlog(codeKerlog);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// Permet de créer un client européen
    /// </summary>
    /// <param name="clientEurope">Le client européen à créer.</param>
    /// <returns>Retourne le client européen créé.</returns>
    [SwaggerResponse(200, "Retourne le client européen créé.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> Post([FromBody] ClientEuropeDto clientEurope)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var createdClientEurope = await _clientEuropeService.Create(clientEurope);
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok(createdClientEurope);
    }
    
    /// <summary>
    /// Permet de mettre à jour un client européen
    /// </summary>
    /// <param name="id">L'identifiant du client européen à mettre à jour.</param>
    /// <param name="clientEurope">Le client européen à mettre à jour.</param>
    /// <returns>Retourne le client européen mis à jour.</returns>
    [SwaggerResponse(200, "Retourne le client européen mis à jour.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpPut("{id:int}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Put(int id, [FromBody] ClientEuropeDto clientEurope)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var updatedClientEurope = await _clientEuropeService.Update(clientEurope);
        if (updatedClientEurope == null)
        {
            return NotFound();
        }
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok(updatedClientEurope);
    }
    
    /// <summary>
    /// permet de supprimer un client européen
    /// </summary>
    /// <param name="id">L'identifiant du client européen à supprimer.</param>
    [SwaggerResponse(200, "Retourne un booléen indiquant si la suppression a réussi.")]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Client européen non trouvé.")]
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _clientEuropeService.DeleteAsync(id, true, false);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok(deleted);
    }

    /// <summary>
    /// Permet de récupérer le groupe d'un client européen
    /// </summary>
    /// <param name="id">L'identifiant du client européen</param>
    /// <returns>Le groupe du client européen</returns>
    [SwaggerResponse(200, "Retourne le groupe du client européen", typeof(GroupDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [HttpGet("group/{id:int}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _clientEuropeService.GetGroup(id);
        return Ok(group);
    }

    /// <summary>
    /// Permet d'ajouter unn client Européen à partir d'un numéro de TVA
    /// </summary>
    /// <param name="vat">Le numéro de TVA du client européen à ajouter.</param>
    /// <returns>Etat de réussite de l'opération</returns>
    [SwaggerResponse(200, "Retourne un booléen indiquant si l'ajout a réussi.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("add_by_vat/{vat}")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> AddByVat(string vat)
    {
        try
        {
            // si le numéro de TVA n'est pas valide, on retourne une erreur
            if(!_vatlayerUtilitiesService.CheckVatNumber(vat))
            {
                return BadRequest();
            }

            var checkClientEurope = await _clientEuropeService.GetByVat(vat);

            // Si le client existe déjà, on retourne une erreur
            if (checkClientEurope != null && !checkClientEurope.IsDeleted && checkClientEurope.Client)
            {
                return BadRequest("Client déjà existant");
            }

            var clientEurope = await _vatlayerUtilitiesService.CreateClientEurope(vat, true, false, true);

            return Ok(clientEurope);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // Map the 404 error to a NotFound response
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
        }
    }

    /// <summary>
    /// Crée un client européen à partir d'un numéro de TVA
    /// </summary>
    /// <param name="vatNumber">Le numéro de TVA du client</param>
    /// <returns>Le client européen créé</returns>
    [SwaggerResponse(200, "Client européen créé", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("create_from_scratch/{vat}")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> CreateFromScratch(string vat)
    {
        var clientEurope = await _clientEuropeService.CreateFromScratchAsync(vat);
        return Ok(clientEurope);
    }
    
    /// <summary>
    /// Supprime un code ERP d'un établissement client européen.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client européen.</param>
    /// <param name="codeType">Le type d'ERP visé</param>
    /// <returns>L'entitée modifiée</returns>
    [SwaggerResponse(200, "Code ERP supprimé", typeof(ClientEurope))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "ClientEurope non trouvé")]
    [HttpPut]
    [Route("code-erp/{id:int}/{codeType}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> DeleteErpCodeAsync(int id, string codeType)
    {
        var result = await _clientEuropeService.DeleteErpCodeAsync(id, codeType);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}