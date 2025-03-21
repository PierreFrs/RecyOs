//  EntrepriseBaseContoller.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/05/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("entreprise_base")]
public class EntrepriseBaseController : BaseApiController
{
    protected readonly  IEntrepriseBaseService _entrepriseBaseService;
    
    public EntrepriseBaseController(IEntrepriseBaseService entrepriseBaseService)
    {
        _entrepriseBaseService = entrepriseBaseService;
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EntrepriseBase à partir de son identifiant
    /// </summary>
    /// <Param name="id">Identifiant de l'entrepriseBase</Param>
    /// <returns>Objet EntrepriseBase</returns>
    [SwaggerResponse(200, "EntrepriseBase trouvé", typeof(EntrepriseBaseDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseBase non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var entrepriseBase = await _entrepriseBaseService.GetById(id);
        if (entrepriseBase == null) return NotFound();
        return Ok(entrepriseBase);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des objets EntrepriseBase
    /// </summary>
    /// <returns>Liste des objets EntrepriseBase</returns>
    [SwaggerResponse(200, "Liste des objets EntrepriseBase", typeof(List<EntrepriseBaseDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EntrepriseBaseGridFilter filter)
    {
        filter = filter ?? new EntrepriseBaseGridFilter();
        var entrepriseBase = await _entrepriseBaseService.GetDataForGrid(filter);
        return Ok(entrepriseBase);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EntrepriseBase à partir de son siren
    /// </summary>
    /// <Param name="siren">Siret de l'entrepriseBase</Param>
    /// <returns>Objet EntrepriseBase</returns>
    [SwaggerResponse(200, "EntrepriseBase trouvé", typeof(EntrepriseBaseDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseBase non trouvé")]
    [HttpGet]
    [Route("siren/{siren}")]
    public async Task<IActionResult> GetBySiren(string siren)
    {
        var entrepriseBase = await _entrepriseBaseService.GetBySiren(siren);
        if (entrepriseBase == null) return NotFound();
        return Ok(entrepriseBase);
    }
    
    /// <summary>
    /// Permet de créer un objet EntrepriseBase
    /// </summary>
    /// <param name="entrepriseBaseDto">Objet à créer</param>
    /// <returns>Objet crée</returns>
    [SwaggerResponse(200, "EntrepriseBase crée", typeof(EntrepriseBaseDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] EntrepriseBaseDto entrepriseBaseDto)
    {
        var entrepriseBase = await _entrepriseBaseService.Edit(entrepriseBaseDto);
        return Ok(entrepriseBase);
    }
    
    /// <summary>
    /// Permet de modifier un objet EntrepriseBase
    /// </summary>
    /// <param name="id">Identifiant de l'objet à modifier</param>
    /// <param name="entrepriseBaseDto">Objet à modifier</param>
    /// <returns>Objet modifié</returns>
    [SwaggerResponse(200, "EntrepriseBase modifié", typeof(EntrepriseBaseDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EntrepriseBaseDto entrepriseBaseDto)
    {
        var entrepriseBase = await _entrepriseBaseService.Edit(entrepriseBaseDto);
        return Ok(entrepriseBase);
    }
    
    /// <summary>
    /// Permet de supprimer un objet EntrepriseBase
    /// </summary>
    /// <param name="id">Identifiant de l'objet à supprimer</param>
    /// <returns></returns>
    [SwaggerResponse(200, "EntrepriseBase supprimé")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        await _entrepriseBaseService.Delete(id);
        return Ok();
    }
    
}