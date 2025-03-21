using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("societes")]
public class SocieteController : BaseApiController
{
    private readonly ISocieteBaseService _societeBaseService;
    public SocieteController(ISocieteBaseService societeBaseService)
    {
        _societeBaseService = societeBaseService;
    }

    /// POST
    /// <summary>
    /// Creates an object of type Societe
    /// </summary>
    /// <returns>Societe</returns>
    [SwaggerResponse(200, "Creates an object of type Societe", typeof(SocieteDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(SocieteDto societeDto)
    {
        var societe = await _societeBaseService.CreateAsync(societeDto);
        if (societe == null) return NotFound();
        return Ok(societe);
    }

    /// GET
    /// <summary>
    /// Gets a list of objects of type Societe
    /// </summary>
    /// <returns>List of Societes</returns>
    [SwaggerResponse(200, "Gets a list of objects of type Societe", typeof(List<SocieteDto>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var societes = await _societeBaseService.GetListAsync();
        return Ok(societes);
    }

    /// GET by Id
    /// <summary>
    /// Gets an object of type Societe from its Id
    /// </summary>
    /// <returns>Societe</returns>
    [SwaggerResponse(200, "Gets an object of type Societe by Id", typeof(SocieteDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var societe = await _societeBaseService.GetByIdAsync(id);
        if (societe == null) return NotFound();
        return Ok(societe);
    }

    /// <summary>
    /// Met à jour une société existante
    /// </summary>
    /// <param name="id">L'identifiant de la société à mettre à jour</param>
    /// <param name="societeDto">Les nouvelles données de la société</param>
    /// <returns>La société mise à jour</returns>
    [SwaggerResponse(200, "Updates an object of type Societe", typeof(SocieteDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromRoute] int id, SocieteDto societeDto)
    {
        var societe = await _societeBaseService.UpdateAsync(id, societeDto);
        if (societe == null) return NotFound();
        return Ok(societe);
    }

    /// DELETE
    /// <summary>
    /// Deletes an object of type Societe from its Id
    /// </summary>
    /// <param name="id">Societe Id</param>
    /// <returns>true</returns>
    [SwaggerResponse(200, "Deletes an object of type Societe")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _societeBaseService.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok(success);
    }

   /// GET for Grid
    /// <summary>
    /// Permet d'obtenir une liste d'objets Societe pour une grille
    /// </summary>
    /// <returns>Liste d'objets Societe</returns>
    [SwaggerResponse(200, "Gets a list of objects of type Societe for a grid", typeof(List<SocieteDto>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("grid")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] SocieteGridFilter filter)
    {
        var societes = await _societeBaseService.GetDataForGrid(filter);
        return Ok(societes);
    }
}