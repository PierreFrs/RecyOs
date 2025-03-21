//  EntrepriseCouvertureController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
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

[Route("entreprise_couverture")]
public class EntrepriseCouvertureController : BaseApiController
{
    protected readonly IEntrepriseCouvertureService _entrepriseCouvertureService;
    
    public EntrepriseCouvertureController(IEntrepriseCouvertureService entrepriseCouvertureService)
    {
        _entrepriseCouvertureService = entrepriseCouvertureService;
    }
    
    /// GET by Id
    /// <summary>
    /// Permet d'obtenir un objet EntrepriseCouverture à partir de son identifiant
    /// </summary>
    /// <Param name="id">Identifiant de l'entrepriseCouverture</Param>
    /// <returns>Objet EntrepriseCouverture</returns>
    [SwaggerResponse(200, "EntrepriseCouverture trouvé", typeof(EntrepriseCouvertureDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseCouverture non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var entrepriseCouverture = await _entrepriseCouvertureService.GetById(id);
        if (entrepriseCouverture == null) return NotFound();
        return Ok(entrepriseCouverture);
    }
    
    /// Get by Siren
    /// <summary>
    /// Permet d'obtenir un objet EntrepriseCouverture à partir de son identifiant entreprise
    /// </summary>
    /// <Param name="siren">Siret de l'entrepriseCouverture</Param>
    /// <returns>Objet EntrepriseCouverture</returns>
    [SwaggerResponse(200, "EntrepriseCouverture trouvé", typeof(EntrepriseCouvertureDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseCouverture non trouvé")]
    [HttpGet]
    [Route("siren/{siren}")]
    public async Task<IActionResult> Get(string siren)
    {
        var entrepriseCouverture = await _entrepriseCouvertureService.GetBySiren(siren);
        if (entrepriseCouverture == null) return NotFound();
        return Ok(entrepriseCouverture);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets EntrepriseCouverture
    /// </summary>
    /// <param name="filter">Filtre de la grille</param>
    /// <returns>Liste des objets EntrepriseCouverture</returns>
    [SwaggerResponse(200, "Liste des objets EntrepriseCouverture", typeof(List<EntrepriseCouvertureDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EntrepriseCouvertureGridFilter filter)
    {
        filter = filter ?? new EntrepriseCouvertureGridFilter();
        var entrepriseCouverture = await _entrepriseCouvertureService.GetDataForGrid(filter);
        return Ok(entrepriseCouverture);
    }

    /// PUT
    /// <summary>
    /// Permet de modifier un objet EntrepriseCouverture
    /// </summary>
    /// <param name="id">Identifiant de l'objet à modifier</param>
    /// <param name="entrepriseCouverture">Objet EntrepriseCouverture</param>
    /// <returns>Objet EntrepriseCouverture modifié</returns>
    [SwaggerResponse(200, "EntrepriseCouverture modifié", typeof(EntrepriseCouvertureDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseCouverture non trouvé")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] EntrepriseCouvertureDto entrepriseCouverture)
    {
        if (entrepriseCouverture == null) return BadRequest();
        if (id != entrepriseCouverture.Id) return BadRequest();
        var entrepriseCouvertureUpdated = await _entrepriseCouvertureService.Edit(entrepriseCouverture);
        if (entrepriseCouvertureUpdated == null) return NotFound();
        return Ok(entrepriseCouvertureUpdated);
    }
    
    /// POST
    /// <summary>
    /// Permet d'ajouter un objet EntrepriseCouverture
    /// </summary>
    /// <param name="entrepriseCouverture">Objet EntrepriseCouverture</param>
    /// <returns>Objet EntrepriseCouverture ajouté</returns>
    [SwaggerResponse(200, "EntrepriseCouverture ajouté", typeof(EntrepriseCouvertureDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Post([FromBody] EntrepriseCouvertureDto entrepriseCouverture)
    {
        if (entrepriseCouverture == null) return BadRequest();
        var entrepriseCouvertureAdded = await _entrepriseCouvertureService.Edit(entrepriseCouverture);
        if (entrepriseCouvertureAdded == null) return BadRequest();
        return Ok(entrepriseCouvertureAdded);
    }
}