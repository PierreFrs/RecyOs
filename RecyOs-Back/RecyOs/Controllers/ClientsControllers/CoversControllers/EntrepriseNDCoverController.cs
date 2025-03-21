using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("entreprise-nd-cover")]
public class EntrepriseNDCoverController : BaseApiController
{
    protected readonly IEntrepriseNDCoverService _entrepriseNDCoverService;
    public EntrepriseNDCoverController(IEntrepriseNDCoverService entrepriseNDCoverService)
    {
        _entrepriseNDCoverService = entrepriseNDCoverService;
    }

    /// GET by Id
    /// <summary>
    /// Gets an object of type EntrepriseNDCover from its Id
    /// </summary>
    /// <Param name="id">Identifiant de l'entrepriseNDCover</Param>
    /// <returns>EntrepriseNDCover</returns>
    [SwaggerResponse(200, "Gets an object of type EntrepriseNDCover by Id", typeof(EntrepriseNDCoverDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var entrepriseNDCover = await _entrepriseNDCoverService.GetById(id);
        if (entrepriseNDCover == null) return NotFound();
        return Ok(entrepriseNDCover);
    }
    
    /// Get by Siren
    /// <summary>
    /// Permet d'obtenir un objet EntrepriseCouverture à partir de son identifiant entreprise
    /// </summary>
    /// <Param name="siren">Siret de l'entrepriseCouverture</Param>
    /// <returns>Objet EntrepriseCouverture</returns>
    [SwaggerResponse(200, "EntrepriseNDCover trouvé", typeof(EntrepriseCouvertureDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EntrepriseNDCover non trouvé")]
    [HttpGet]
    [Route("siren/{siren}")]
    public async Task<IActionResult> Get(string siren)
    {
        
        var entrepriseNDCover = await _entrepriseNDCoverService.GetBySiren(siren);
        if (entrepriseNDCover == null) return NotFound();
        return Ok(entrepriseNDCover);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets EntrepriseCouverture
    /// </summary>
    /// <param name="filter">Filtre de la grille</param>
    /// <returns>Liste des objets EntrepriseCouverture</returns>
    [SwaggerResponse(200, "Liste des objets EntrepriseNDCover", typeof(List<EntrepriseNDCoverDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EntrepriseNDCoverGridFilter filter)
    {
        filter = filter ?? new EntrepriseNDCoverGridFilter();
        var entrepriseNDCover = await _entrepriseNDCoverService.GetDataForGrid(filter);
        return Ok(entrepriseNDCover);
    }


    /// <summary>
    /// Updates an object of type EntrepriseNDCover.
    /// </summary>
    /// <param name="id">The ID of the object to update.</param>
    /// <param name="entrepriseNDCoverDto">The updated EntrepriseNDCoverDto object.</param>
    /// <returns>
    /// Returns an IActionResult representing the updated EntrepriseNDCoverDto object.
    /// Returns a 400 Bad Request status code if entrepriseNDCoverDto is null or if the ID does not match.
    /// Returns a 404 Not Found status code if no object is found with the provided ID.
    /// Returns a 200 OK status code with the updated EntrepriseNDCoverDto object if the update is successful.
    /// </returns>
    [SwaggerResponse(200, "Updates an object of type EntrepriseNDCover", typeof(EntrepriseNDCoverDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] EntrepriseNDCoverDto entrepriseNDCoverDto)
    {
        if (entrepriseNDCoverDto == null) return BadRequest();
        if (id != entrepriseNDCoverDto.Id) return BadRequest();
        var entrepriseNDCoverDtoUpdated = await _entrepriseNDCoverService.Edit(entrepriseNDCoverDto);
        if (entrepriseNDCoverDtoUpdated == null) return NotFound();
        return Ok(entrepriseNDCoverDtoUpdated);
    }
    
    
    /// POST
    /// <summary>
    /// Creates an object of type EntrepriseNDCover
    /// </summary>
    /// <returns>EntrepriseNDCover</returns>
    [SwaggerResponse(200, "Creates an object of type EntrepriseNDCover", typeof(EntrepriseNDCoverDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EntrepriseNDCoverDto entrepriseNDCoverDto)
    {
        if (entrepriseNDCoverDto == null) return BadRequest();
        var entrepriseNDCoverDtoAdded = await _entrepriseNDCoverService.Edit(entrepriseNDCoverDto);
        if (entrepriseNDCoverDtoAdded == null) return BadRequest();
        return Ok(entrepriseNDCoverDtoAdded);
    }
}