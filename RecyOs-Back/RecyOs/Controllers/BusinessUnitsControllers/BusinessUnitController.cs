using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[ApiController]
[Route("business-unit")]
public class BusinessUnitController : BaseApiController
{
    private readonly IBusinessUnitService<BusinessUnitDto> _businessUnitService;
    public BusinessUnitController(IBusinessUnitService<BusinessUnitDto> businessUnitService)
    {
        _businessUnitService = businessUnitService;
    }

    /// GET
    /// <summary>
    /// Gets a list of objects of type BusinessUnit
    /// </summary>
    /// <returns>List of BusinessUnits</returns>
    [SwaggerResponse(200, "Gets a list of objects of type BusinessUnit", typeof(List<BusinessUnitDto>))]
    [SwaggerResponse(401, "Unauthorized")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var businessUnits = await _businessUnitService.GetListAsync();
        return Ok(businessUnits);
    }

    /// GET by Id
    /// <summary>
    /// Gets an object of type BusinessUnit from its Id
    /// </summary>
    /// <returns>BusinessUnit</returns>
    [SwaggerResponse(200, "Gets an object of type BusinessUnit by Id", typeof(BusinessUnitDto))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var businessUnit = await _businessUnitService.GetByIdAsync(id);
        if (businessUnit == null) return NotFound();
        return Ok(businessUnit);
    }
}