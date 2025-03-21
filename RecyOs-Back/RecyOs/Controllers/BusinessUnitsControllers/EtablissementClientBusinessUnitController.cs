using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[ApiController]
[Route("etablissement-client-business-unit")]
public class EtablissementClientBusinessUnitController : BaseApiController
{
    private readonly IEtablissementClientBusinessUnitService<EtablissementClientBusinessUnitDto, BusinessUnitDto> _etablissementClientBusinessUnitService;
    public EtablissementClientBusinessUnitController(IEtablissementClientBusinessUnitService<EtablissementClientBusinessUnitDto, BusinessUnitDto> etablissementClientBusinessUnitService)
    {
        _etablissementClientBusinessUnitService = etablissementClientBusinessUnitService;
    }

    /// POST
    /// <summary>
    /// Adds a new BU to an EtablissementClient
    /// </summary>
    /// <returns>EtablissementClientBusinessUnit</returns>
    [SwaggerResponse(200, "Creates an object of type EtablissementClientBusinessUnit",
        typeof(EtablissementClientBusinessUnitDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Create([FromBody] EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto)
    {
        var etablissementClientBusinessUnit = await _etablissementClientBusinessUnitService.CreateAsync(etablissementClientBusinessUnitDto);
        if (etablissementClientBusinessUnit == null) return NotFound();
        return Ok(etablissementClientBusinessUnit);
    }

    /// GET by EtablissementClientId
    /// <summary>
    /// Gets a list of BusinessUnitObject objects from an EtablissementClientId
    /// </summary>
    /// <returns>EtablissementClientBusinessUnit</returns>
    [SwaggerResponse(200, "Gets an object of type EtablissementClientBusinessUnit by Id", typeof(EtablissementClientBusinessUnitDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("{etablissementClientId:int}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByClientId(int etablissementClientId)
    {
        var businessUnitList = await _etablissementClientBusinessUnitService.GetByEtablissementClientIdAsync(etablissementClientId);
        if (businessUnitList == null) return NotFound();
        return Ok(businessUnitList);
    }

    /// <summary>
    /// Deletes an object of type EtablissementClientBusinessUnit from the EtablissementClientId and BusinessUnitId
    /// </summary>
    /// <param name="etablissementClientBusinessUnitDto"></param>
    /// <returns></returns>
    [SwaggerResponse(200, "Deletes an object of type EtablissementClientBusinessUnit")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    [HttpDelete]
    [Route("")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Delete([FromBody] EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto)
    {
        if (etablissementClientBusinessUnitDto == null) return NotFound();
        var success = await _etablissementClientBusinessUnitService.DeleteAsync(etablissementClientBusinessUnitDto);
        return Ok(success);
    }
}