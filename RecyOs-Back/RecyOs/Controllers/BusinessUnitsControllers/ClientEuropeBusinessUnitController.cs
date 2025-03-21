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
[Route("client-europe-business-unit")]
public class ClientEuropeBusinessUnitController : BaseApiController
{
    private readonly IClientEuropeBusinessUnitService<ClientEuropeBusinessUnitDto, BusinessUnitDto> _clientEuropeBusinessUnitService;
    public ClientEuropeBusinessUnitController(IClientEuropeBusinessUnitService<ClientEuropeBusinessUnitDto, BusinessUnitDto> clientEuropeBusinessUnitService)
    {
        _clientEuropeBusinessUnitService = clientEuropeBusinessUnitService;
    }

    /// POST
    /// <summary>
    /// Adds a new BU to an ClientEurope
    /// </summary>
    /// <returns>ClientEuropeBusinessUnit</returns>
    [SwaggerResponse(200, "Creates an object of type ClientEuropeBusinessUnit",
        typeof(ClientEuropeBusinessUnitDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Create([FromBody] ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto)
    {
        var clientEuropeBusinessUnit = await _clientEuropeBusinessUnitService.CreateAsync(clientEuropeBusinessUnitDto);
        if (clientEuropeBusinessUnit == null) return NotFound();
        return Ok(clientEuropeBusinessUnit);
    }

    /// GET by ClientEuropeId
    /// <summary>
    /// Gets a list of BusinessUnitObject objects from an ClientEuropeId
    /// </summary>
    /// <returns>ClientEuropeBusinessUnit</returns>
    [SwaggerResponse(200, "Gets an object of type ClientEuropeBusinessUnit by Id", typeof(ClientEuropeBusinessUnitDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("{clientEuropeId:int}")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetByClientId(int clientEuropeId)
    {
        var businessUnitList = await _clientEuropeBusinessUnitService.GetByClientEuropeIdAsync(clientEuropeId);
        if (businessUnitList == null) return NotFound();
        return Ok(businessUnitList);
    }

    /// <summary>
    /// Deletes an object of type ClientEuropeBusinessUnit from the ClientEuropeId and BusinessUnitId
    /// </summary>
    /// <param name="clientEuropeBusinessUnitDto"></param>
    /// <returns></returns>
    [SwaggerResponse(200, "Deletes an object of type ClientEuropeBusinessUnit")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Vous n'avez pas les droits nécessaire pour effectuer cette action")]
    [SwaggerResponse(404, "Not Found")]
    [HttpDelete]
    [Route("")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> Delete([FromBody] ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto)
    {
        if (clientEuropeBusinessUnitDto == null) return NotFound();
        var success = await _clientEuropeBusinessUnitService.DeleteAsync(clientEuropeBusinessUnitDto);
        return Ok(success);
    }
}