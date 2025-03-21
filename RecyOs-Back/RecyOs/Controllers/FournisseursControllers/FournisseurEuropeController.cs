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
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.vatlayer;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("fournisseur_europe")]
public class FournisseurEuropeController : BaseApiController
{
    private readonly IFournisseurEuropeService _fournisseurEuropeService;
    private readonly ISynchroWaitingToken _engineWaitingToken;
    private readonly IVatlayerUtilitiesService _vatlayerUtilitiesService;
    private readonly IConfiguration _config;
    
    public FournisseurEuropeController(IFournisseurEuropeService fournisseurEuropeService, ISynchroWaitingToken synchroService,
        IVatlayerUtilitiesService vatlayerUtilitiesService, IConfiguration config)
    {
        _fournisseurEuropeService = fournisseurEuropeService;
        _engineWaitingToken = synchroService;
        _vatlayerUtilitiesService = vatlayerUtilitiesService;
        _config = config;
    }
    
    /// <summary>
    /// Retrieves a European Provider by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the European Provider to retrieve.</param>
    /// <returns>Returns a DTO of the European Provider corresponding to the given identifier.</returns>
    [SwaggerResponse(200, "Fournisseur Europe trouvé", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Fournisseur Europe non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> Get(int id)
    {
        var clientEurope = await _fournisseurEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound();
        }
        return Ok(clientEurope);
    }

    /// <summary>
    /// Retrieves a list of ClientEuropeDto objects based on the specified filter.
    /// </summary>
    /// <param name="filter">The ClientEuropeGridFilter object to filter the results.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult object representing the HTTP response.</returns>
    [SwaggerResponse(200, "Retourne une liste de Fournisseur Europe.", typeof(IEnumerable<ClientEuropeDto>))]
    [SwaggerResponse(401, "Non autorisé.")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] ClientEuropeGridFilter filter)
    {
        filter = filter ?? new ClientEuropeGridFilter();
        var clientEurope = await _fournisseurEuropeService.GetGridDataAsync(filter);
        return Ok(clientEurope);
    }

    /// <summary>
    /// Retrieves a European supplier by VAT number.
    /// </summary>
    /// <param name="vat">The VAT number of the supplier.</param>
    /// <returns>The European supplier information if found; otherwise, returns a 404 status.</returns>
    [SwaggerResponse(200, "Retourne un fournisseur européen.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "Fournisseur européen non trouvé.")]
    [HttpGet("vat/{vat}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByVat(string vat)
    {
        var fournisseurEurope = await _fournisseurEuropeService.GetByVat(vat);
        if (fournisseurEurope == null) return NotFound();
        return Ok(fournisseurEurope);
    }

    /// <summary>
    /// Creates a new Fournisseur Europe.
    /// </summary>
    /// <param name="fournisseurEurope">The Fournisseur Europe to create.</param>
    /// <returns>The created Fournisseur Europe.</returns>
    /// <remarks>
    /// This method creates a new Fournisseur Europe using the provided Fournisseur Europe DTO.
    /// It requires the "CreateFournisseur" authorization policy.
    /// </remarks>
    [SwaggerResponse(200, "Fournisseur Europe créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> Create([FromBody] ClientEuropeDto fournisseurEurope)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var clientEurope = await _fournisseurEuropeService.Create(fournisseurEurope);
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok(clientEurope);
    }

    /// <summary>
    /// Updates a European supplier.
    /// </summary>
    /// <param name="id">The ID of the supplier to update.</param>
    /// <param name="fournisseurEurope">The updated supplier Europe information.</param>
    /// <returns>The updated supplier Europe.</returns>
    [SwaggerResponse(200, "Retourne le fournisseur européen mis à jour.", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé.")]
    [SwaggerResponse(404, "fournisseur européen non trouvé.")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> Put(int id, [FromBody] ClientEuropeDto fournisseurEurope)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var updatedFournisseurEurope = await _fournisseurEuropeService.Update(fournisseurEurope);
        if (updatedFournisseurEurope == null)
        {
            return NotFound();
        }
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok(updatedFournisseurEurope);
    }

    /// <summary>
    /// Supprime un fournisseur européen en fonction de son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du fournisseur européen à supprimer.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    /// <response code="200">Fournisseur Europe supprimé</response>
    /// <response code="401">Non autorisé</response>
    [SwaggerResponse(200, "Fournisseur Europe supprimé")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Fournisseur Europe non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var deleted = await _fournisseurEuropeService.DeleteAsync(id, false, true);
        if (!deleted)
        {
            return NotFound();
        }
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok();
    }

    /// <summary>
    /// Retrieves a European supplier by its Kerlog code.
    /// </summary>
    /// <param name="codeKerlog">The Kerlog code of the supplier.</param>
    /// <returns>The European supplier with the specified Kerlog code.</returns>
    [SwaggerResponse(200, "Fournisseur Europe trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Fournisseur Europe non trouvé")]
    [HttpGet]
    [Route("code_kerlog/{codeKerlog}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByCodeKerlog(string codeKerlog)
    {
        var fournisseurEurope = await _fournisseurEuropeService.GetByCodeKerlog(codeKerlog);
        if (fournisseurEurope == null)
        {
            return NotFound();
        }
        return Ok(fournisseurEurope);
    }

    /// <summary>
    /// Retrieves a European supplier by code MKGT.
    /// </summary>
    /// <param name="codeMkgt">The MKGT code.</param>
    /// <returns>The European supplier.</returns>
    /// <response code="200">European supplier found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">European supplier not found.</response>
    [SwaggerResponse(200, "Fournisseur Europe trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Fournisseur Europe non trouvé")]
    [HttpGet]
    [Route("code_mkgt/{codeMkgt}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByCodeMkgt(string codeMkgt)
    {
        var fournisseurEurope = await _fournisseurEuropeService.GetByCodeMkgt(codeMkgt);
        if (fournisseurEurope == null)
        {
            return NotFound();
        }
        return Ok(fournisseurEurope);
    }

    /// <summary>
    /// Retrieves a European supplier by Odoo ID.
    /// </summary>
    /// <param name="idOdoo">The Odoo ID of the supplier.</param>
    /// <returns>The European supplier with the specified Odoo ID.</returns>
    [SwaggerResponse(200, "Fournisseur Europe trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Fournisseur Europe non trouvé")]
    [HttpGet]
    [Route("id_odoo/{idOdoo}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByOdooId(string idOdoo)
    {
        var fournisseurEurope = await _fournisseurEuropeService.GetByIdOdoo(idOdoo);
        if (fournisseurEurope == null)
        {
            return NotFound();
        }
        return Ok(fournisseurEurope);
    }

    /// <summary>
    /// Adds a client with the specified VAT number.
    /// </summary>
    /// <param name="vat">The VAT number of the client to add.</param>
    /// <returns>The newly created client information.</returns>
    /// <remarks>
    /// If the VAT number is not valid, a bad request will be returned.
    /// If a client with the same VAT number already exists, a bad request will be returned.
    /// </remarks>
    [SwaggerResponse(200, "Fournisseur Europe créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "Non trouvé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("add_by_vat/{vat}")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> AddByVat(string vat)
    {
        try
        {
            // si le numéro de TVA n'est pas valide, on retourne une erreur
            if(!_vatlayerUtilitiesService.CheckVatNumber(vat))
            {
                return BadRequest();
            }

            var fournisseurEurope = await _fournisseurEuropeService.GetByVat(vat);

            // si le fournisseur européen existe déjà, on retourne une erreur
            if (fournisseurEurope != null && !fournisseurEurope.IsDeleted && fournisseurEurope.Fournisseur)
            {
                return BadRequest("L'etablissement Fournisseur existe déjà");
            }

            var clientEurope = await _vatlayerUtilitiesService.CreateClientEurope(vat, false, true, true);

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
    /// Crée un Etablissement Fournisseur européen contenant uniquement un VAT
    /// </summary>
    /// <param name="vat">L'identifiant administratif de l'établissement</param>
    /// <returns>Un fournisseur vide</returns>
    [SwaggerResponse(200, "EtablissementFournisseur créé", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("create_from_scratch/{siret}")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> CreateFromScratch(string vat)
    {
        var fournisseurEuropeen = await _fournisseurEuropeService.CreateFromScratchAsync(vat);
        return Ok(fournisseurEuropeen);
    }

    /// <summary>
    /// Supprime un code ERP d'un établissement fournisseur.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement fournisseur.</param>
    /// <param name="codeType">Le type d'ERP visé</param>
    /// <returns>L'entitée modifiée</returns>
    [SwaggerResponse(200, "Code ERP supprimé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "ClientEurope non trouvé")]
    [HttpPut]
    [Route("code-erp/{id:int}/{codeType}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> DeleteErpCodeAsync(int id, string codeType)
    {
        var result = await _fournisseurEuropeService.DeleteErpCodeAsync(id, codeType);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}