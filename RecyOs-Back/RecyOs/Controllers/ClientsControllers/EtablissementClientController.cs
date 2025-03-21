//  EtablissementClientController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 07/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using RecyOs.Engine;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Requests;
using RecyOs.ORM.Results;
using RecyOs.ORM.Service.pappers;
using Swashbuckle.AspNetCore.Annotations;
using IEtablissementClientService = RecyOs.ORM.Interfaces.hub.IEtablissementClientService;

namespace RecyOs.Controllers;
[Route("etablissement_client")]
public class EtablissementClientController : BaseApiController
{
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IPappersUtilitiesService _pappersUtilitiesService;
    private readonly ISynchroWaitingToken _engineWaitingToken;
    private readonly IConfiguration _config;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    
    public EtablissementClientController(IEtablissementClientService etablissementClientService,
        IPappersUtilitiesService pappersUtilitiesService, ISynchroWaitingToken synchroService
        , IConfiguration config)
    {
        _etablissementClientService = etablissementClientService;
        _pappersUtilitiesService = pappersUtilitiesService;
        _engineWaitingToken = synchroService;
        _config = config;
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementClient à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant de l'etablissementClient</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "EtablissementClient trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> Get(int id)
    {
        var etablissementClient = await _etablissementClientService.GetById(id);
        if (etablissementClient == null) return NotFound();
        return Ok(etablissementClient);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des objets EtablissementClient
    /// </summary>
    /// <returns>Liste des objets EtablissementClient</returns>
    [SwaggerResponse(200, "Liste des objets EtablissementClient", typeof(List<EtablissementClientDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EtablissementClientGridFilter filter)
    {
        filter = filter ?? new EtablissementClientGridFilter();
        var etablissementClient = await _etablissementClientService.GetDataForGrid(filter);
        return Ok(etablissementClient);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementClient à partir de son siret
    /// </summary>
    /// <param name="siret">Siret de l'etablissementClient</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "EtablissementClient trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpGet]
    [Route("siret/{siret}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetBySiret(string siret)
    {
        var etablissementClient = await _etablissementClientService.GetBySiret(siret);
        if (etablissementClient == null) return NotFound();
        return Ok(etablissementClient);
    }

    /// <summary>
    /// Récupère le groupe associé à un établissement client
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client</param>
    /// <returns>Le groupe associé</returns>
    [SwaggerResponse(200, "Groupe trouvé", typeof(GroupDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Groupe non trouvé")]
    [HttpGet]
    [Route("{id:int}/group")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _etablissementClientService.GetGroup(id);
        if (group == null) return NotFound();
        return Ok(group);
    }

    /// <summary>
    /// Permet de créer un objet EtablissementClient
    /// </summary>
    /// <param name="etablissementClient">Objet EtablissementClient à créer</param>
    /// <returns>Objet EtablissementClient créé</returns>
    [SwaggerResponse(200, "EtablissementClient créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> Create([FromBody] EtablissementClientDto etablissementClient)
    {
        var etablissementClientCreated = await _etablissementClientService.Edit(etablissementClient);
        return Ok(etablissementClientCreated);
    }
    
    /// <summary>
    /// Permet de modifier un objet EtablissementClient
    /// </summary>
    /// <param name="id">id de l'établissement client à modifier</param>
    /// <param name="etablissementClient">Objet EtablissementClient à modifier</param>
    /// <returns>Objet EtablissementClient modifié</returns>
    [SwaggerResponse(200, "EtablissementClient modifié", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(480, "clé en double dans l'objet 'dbo.EtablissementClient' avec un index unique 'IX_EtablissementClient_code_mkgt'")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Edit(int id, EtablissementClientDto etablissementClient)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        EtablissementClientDto result;
        if (id != etablissementClient.Id)
        {
            return BadRequest("Les identifiants ne correspondent pas");
        }

        try
        {
            result = await _etablissementClientService.Edit(etablissementClient);
        }
        catch (Exception e)
        {
            // Cas où le code MKGT est déjà utilisé
            if (e.InnerException?.InnerException?.InnerException?.Message.Contains("Cannot insert duplicate key row in object 'dbo.EtablissementClient' with unique index 'IX_EtablissementClient_code_mkgt'.") == true)
            {
                return StatusCode(480, "clé en double dans l'objet 'dbo.EtablissementClient' avec un index unique 'IX_EtablissementClient_code_mkgt'");
            }
            else
            {
                return StatusCode(500, "Erreur webService");
            }
        }
        
        if(sync)_engineWaitingToken.StopWaiting();
        return Ok(result);
    }
    
    /// <summary>
    /// Permet de modifier un objet EtablissementClient
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [SwaggerResponse(200, "EtablissementClient modifié", typeof(ServiceResult))]
    [SwaggerResponse(404, "Non trouvé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPut]
    [Route("change-siret/{id:int}")]
    [Authorize(Policy = "UpdateSiret")]
    public async Task<IActionResult> ChangeSiret([FromRoute] int id, [FromBody] SiretUpdateRequest request)
    {
        _logger.Info($"ChangeSiret : {id} - {request.Siret}");
        var newSiret = request.Siret;
        if (string.IsNullOrEmpty(newSiret))
        {
            return BadRequest("Siret is required.");
        }
        
        var result = await _etablissementClientService.ChangeSiretAsync(id, newSiret, true, false, new ContextSession());
    
        if (!result.Success)
        {
            return result.StatusCode == StatusCodes.Status404NotFound
                ? NotFound(result.Message)
                : BadRequest(result.Message);
        }

        return Ok(result);
    }

    /// <summary>
    /// Permet de supprimer un objet EtablissementClient
    /// </summary>
    /// <param name="id">Identifiant de l'etablissementClient</param>
    /// <param name="estClient">Indique si l'etablissementClient est un client</param>
    /// <param name="estFournisseur">Indique si l'etablissementClient est un fournisseur</param>
    /// <returns>Etat de réussite de l'opération</returns>  
    [SwaggerResponse(200, "EtablissementClient supprimé")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, bool estClient = true, bool estFournisseur = false)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var deleted = await _etablissementClientService.DeleteAsync(id, estClient, estFournisseur);
        if (!deleted)
        {
            return NotFound();
        }
        if(sync)_engineWaitingToken.StopWaiting();
        return Ok();
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementClient à partir de son code kerlog
    /// </summary>
    /// <param name="codeKerlog">Code kerlog de l'etablissementClient</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "EtablissementClient trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpGet]
    [Route("code_kerlog/{codeKerlog}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByCodeKerlog(string codeKerlog)
    {
        var etablissementClient = await _etablissementClientService.GetByCodeKerlog(codeKerlog);
        if (etablissementClient == null) return NotFound();
        return Ok(etablissementClient);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementClient à partir de son code mkgt
    /// </summary>
    /// <param name="codeMkgt">Code mkgt de l'etablissementClient</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "EtablissementClient trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpGet]
    [Route("code_mkgt/{codeMkgt}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByCodeMkgt(string codeMkgt)
    {
        var etablissementClient = await _etablissementClientService.GetByCodeMkgt(codeMkgt);
        if (etablissementClient == null) return NotFound();
        return Ok(etablissementClient);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementClient à partir de son id odoo
    /// </summary>
    /// <param name="idOdoo">Id odoo de l'etablissementClient</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "EtablissementClient trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpGet]
    [Route("id_odoo/{idOdoo}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByOdooId(string idOdoo)
    {
        var etablissementClient = await _etablissementClientService.GetByIdOdoo(idOdoo);
        if (etablissementClient == null) return NotFound();
        return Ok(etablissementClient);
    }

    /// <summary>
    /// Permet d'ajouter un EtablissementClient par son suméro de siret
    /// Les données sont récupérées depuis les API de pappers
    /// </summary>
    /// <param name="siret">Siret de l'etablissementClient</param>
    /// <returns>Etat de réussite de l'opération</returns>
    [SwaggerResponse(200, "EtablissementClient créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("add_by_siret/{siret}")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> AddBySiret(string siret)
    {
        try
        {
            // Si le siret n'est pas valide, on retourne une erreur
            if (!_pappersUtilitiesService.CheckSiret(siret))
            {
                return BadRequest("Le siret n'est pas valide");
            }

            var checkEtablissementClient = await _etablissementClientService.GetBySiret(siret);

            // Si l'etablissementClient existe déjà, on ne fait rien et on retourne une erreur
            if (checkEtablissementClient != null && !checkEtablissementClient.IsDeleted &&
                checkEtablissementClient.Client)
            {
                return BadRequest("L'etablissementClient existe déjà");
            }

            var etablissementClient =
                await _pappersUtilitiesService.CreateEtablissementClientBySiret(siret, true, false);

            return Ok(etablissementClient);
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
    /// Crée un Etablissement Client contenant uniquement un SIRET
    /// </summary>
    /// <param name="siret">L'identifiant administratif de l'établissement</param>
    /// <returns>Un client vide</returns>
    [SwaggerResponse(200, "EtablissementClient créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("create_from_scratch/{siret}")]
    [Authorize(Policy = "CreateClient")]
    public async Task<IActionResult> CreateFromScratch(string siret)
    {
        var etablissementClient = await _etablissementClientService.CreateFromScratchAsync(siret);
        return Ok(etablissementClient);
    }

    /// <summary>
    /// Supprime un code ERP d'un établissement client.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <param name="codeType">Le type d'ERP visé</param>
    /// <returns>L'entitée modifiée</returns>
    [SwaggerResponse(200, "Code ERP supprimé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpPut]
    [Route("code-erp/{id:int}/{codeType}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> DeleteErpCodeAsync(int id, string codeType)
    {
        var result = await _etablissementClientService.DeleteErpCodeAsync(id, codeType);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}