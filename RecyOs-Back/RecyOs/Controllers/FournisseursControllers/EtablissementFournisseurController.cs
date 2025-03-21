//  EtablissementFournisseurController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 15/02/2024
// Fichier Modifié le : 05/03/2024
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
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Requests;
using RecyOs.ORM.Results;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;
[Route("etablissement_fournisseur")]

public class EtablissementFournisseurController : BaseApiController
{
    private readonly IEtablissementFournisseurService _etablissementFournisseurService;
    private readonly IPappersUtilitiesService _pappersUtilitiesService;
    private readonly ISynchroWaitingToken _engineWaitingToken;
    private readonly IConfiguration _config;
    
    public EtablissementFournisseurController(IEtablissementFournisseurService etablissementFournisseurService, 
        IPappersUtilitiesService pappersUtilitiesService, ISynchroWaitingToken synchroWaitingToken, IConfiguration config)
    {
        _etablissementFournisseurService = etablissementFournisseurService;
        _pappersUtilitiesService = pappersUtilitiesService;
        _engineWaitingToken = synchroWaitingToken;
        _config = config;
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Etablissement Fournisseur à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant de l'etablissement Fournisseur</param>
    /// <returns>Objet Etablissement Fournisseur</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement Fournisseur non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> Get(int id)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.GetById(id);
        if (etablissementFournisseur == null) return NotFound();
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des objets Etablissement Fournisseur
    /// </summary>
    /// <returns>Liste des objets Etablissement Fournisseur</returns>
    [SwaggerResponse(200, "Liste des objets EtablissementClient", typeof(List<EtablissementClientDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EtablissementClientGridFilter filter)
    {
        filter = filter ?? new EtablissementClientGridFilter();
        var etablissementFournisseur = await _etablissementFournisseurService.GetDataForGrid(filter);
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Etablissement Fournisseur à partir de son siret
    /// </summary>
    /// <param name="siret">Siret de l'etablissement Fournisseur</param>
    /// <returns>Objet Etablissement Fournisseur</returns>
    [SwaggerResponse(200, "Etablissement Fournissuer trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement fournisseur non trouvé")]
    [HttpGet]
    [Route("siret/{siret}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetBySiret(string siret)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.GetBySiret(siret);
        if (etablissementFournisseur == null) return NotFound();
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet de créer un objet Etablissement Fournisseur
    /// </summary>
    /// <param name="etablissementFournisseur">Objet Etablissement Fournisseur à créer</param>
    /// <returns>Objet Etablissement Fournisseur créé</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> Create([FromBody] EtablissementClientDto etablissementFournisseur)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var etablissementFournisseurCreated = await _etablissementFournisseurService.Edit(etablissementFournisseur);
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok(etablissementFournisseurCreated);
    }
    
    /// <summary>
    /// Permet de modifier un objet EtablissementFournisseur
    /// </summary>
    /// <param name="id">id de l'établissement fournisseur à modifier</param>
    /// <param name="etablissementFournisseur">Objet EtablissementFournisseur à modifier</param>
    /// <returns>Objet EtablissementFournisseur modifié</returns>
    [SwaggerResponse(200, "Etablissement fournisseur modifié", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(480, "clé en double dans l'objet 'dbo.EtablissementClient' avec un index unique 'IX_EtablissementClient_code_mkgt'")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> Edit(int id, EtablissementClientDto etablissementFournisseur)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        EtablissementClientDto result;
        if (id != etablissementFournisseur.Id)
        {
            return BadRequest("Les identifiants ne correspondent pas");
        }

        try
        {
            result = await _etablissementFournisseurService.Edit(etablissementFournisseur);
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
    /// Permet de modifier un objet Etablissement fournisseur
    /// </summary>
    /// <param name="id">id de l'établissement fournisseur à modifier</param>
    /// <param name="request">Objet EtablissementClient à modifier</param>
    /// <returns>Objet EtablissementClient modifié</returns>
    [SwaggerResponse(200, "EtablissementClient modifié", typeof(ServiceResult))]
    [SwaggerResponse(404, "Non trouvé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPut]
    [Route("change-siret/{id:int}")]
    [Authorize(Policy = "UpdateSiret")]
    public async Task<IActionResult> ChangeSiret([FromRoute] int id, [FromBody] SiretUpdateRequest request)
    {
        var newSiret = request.Siret;
        if (string.IsNullOrEmpty(newSiret))
        {
            return BadRequest("Siret is required.");
        }
        
        var result = await _etablissementFournisseurService.ChangeSiretAsync(id, newSiret, false, true, new ContextSession());
    
        if (!result.Success)
        {
            return result.StatusCode == StatusCodes.Status404NotFound
                ? NotFound(result.Message)
                : BadRequest(result.Message);
        }

        return Ok(result);
    }
    
    /// <summary>
    /// Permet de supprimer un objet Etablissement Fournisseurs
    /// </summary>
    /// <param name="id">Identifiant de l'etablissement Fournisseur</param>
    [SwaggerResponse(200, "Etablissement Fournisseur supprimé")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement Fournisseur non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        bool sync = _config.GetValue<bool>("engine:writeSync");
        var deleted = await _etablissementFournisseurService.DeleteAsync(id, false, true);
        if (!deleted)
        {
            return NotFound();
        }
        if(sync) _engineWaitingToken.StopWaiting();
        return Ok();
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Etablissement Fournisseur à partir de son code kerlog
    /// </summary>
    /// <param name="codeKerlog">Code kerlog de l'etablissement Fournisseur</param>
    /// <returns>Objet Etablissement Fournisseur</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement Fournisseur non trouvé")]
    [HttpGet]
    [Route("code_kerlog/{codeKerlog}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByCodeKerlog(string codeKerlog)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.GetByCodeKerlog(codeKerlog);
        if (etablissementFournisseur == null) return NotFound();
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Etablissement Fournisseur à partir de son code mkgt
    /// </summary>
    /// <param name="codeMkgt">Code mkgt de l'etablissement Fournisseur</param>
    /// <returns>Objet Etablissement Fournisseur</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement Fournisseur non trouvé")]
    [HttpGet]
    [Route("code_mkgt/{codeMkgt}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByCodeMkgt(string codeMkgt)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.GetByCodeMkgt(codeMkgt);
        if (etablissementFournisseur == null) return NotFound();
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Etablissement Fournisseur à partir de son id odoo
    /// </summary>
    /// <param name="idOdoo">Id odoo de l'etablissement Fournisseur</param>
    /// <returns>Objet EtablissementClient</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur trouvé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement Fournisseur non trouvé")]
    [HttpGet]
    [Route("id_odoo/{idOdoo}")]
    [Authorize(Policy = "ReadFournisseur")]
    public async Task<IActionResult> GetByOdooId(string idOdoo)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.GetByIdOdoo(idOdoo);
        if (etablissementFournisseur == null) return NotFound();
        return Ok(etablissementFournisseur);
    }
    
    /// <summary>
    /// Permet d'ajouter un Etablissement Fournisseur par son suméro de siret
    /// Les données sont récupérées depuis les API de pappers
    /// </summary>
    /// <param name="siret">Siret de l'etablissement Fournisseur</param>
    /// <returns>Etat de réussite de l'opération</returns>
    [SwaggerResponse(200, "Etablissement Fournisseur créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("add_by_siret/{siret}")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> AddBySiret(string siret)
    {
        try
        {
            // Si le siret n'est pas valide, on retourne une erreur
            if (!_pappersUtilitiesService.CheckSiret(siret))
            {
                return BadRequest("Le siret n'est pas valide");
            }

            var checkEtablissementFournisseur = await _etablissementFournisseurService.GetBySiret(siret);

            // Si l'etablissement Fournisseur existe déjà, on ne fait rien et on retourne une erreur
            if (checkEtablissementFournisseur != null && !checkEtablissementFournisseur.IsDeleted && checkEtablissementFournisseur.Fournisseur)
            {
                return BadRequest("L'etablissement Fournisseur existe déjà");
            }

            var etablissementFournisseur = await _pappersUtilitiesService.CreateEtablissementClientBySiret(siret, false, true);

            return Ok(etablissementFournisseur);
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
    /// Crée un Etablissement Fournisseur contenant uniquement un SIRET
    /// </summary>
    /// <param name="siret">L'identifiant administratif de l'établissement</param>
    /// <returns>Un fournisseur vide</returns>
    [SwaggerResponse(200, "EtablissementFournisseur créé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpPost]
    [Route("create_from_scratch/{siret}")]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> CreateFromScratch(string siret)
    {
        var etablissementFournisseur = await _etablissementFournisseurService.CreateFromScratchAsync(siret);
        return Ok(etablissementFournisseur);
    }

    /// <summary>
    /// Supprime un code ERP d'un établissement fournisseur.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement fournisseur.</param>
    /// <param name="codeType">Le type d'ERP visé</param>
    /// <returns>L'entitée modifiée</returns>
    [SwaggerResponse(200, "Code ERP supprimé", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpPut]
    [Route("code-erp/{id:int}/{codeType}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> DeleteErpCodeAsync(int id, string codeType)
    {
        var result = await _etablissementFournisseurService.DeleteErpCodeAsync(id, codeType);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}