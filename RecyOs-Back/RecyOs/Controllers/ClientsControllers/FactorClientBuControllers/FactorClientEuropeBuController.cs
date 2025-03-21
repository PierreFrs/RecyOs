// <copyright file="FactorClientEuropeBuController.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;
[Route("factor_client_europe_bu")]

public class FactorClientEuropeBuController : BaseApiController
{
    private readonly IFactorClientEuropeBuService _service;

    public FactorClientEuropeBuController(IFactorClientEuropeBuService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Permet d'obtenir la liste de tous les objets FactorClientEuropeBu
    /// </summary>
    /// <returns>Liste d'objets FactorClientEuropeBu</returns>
    [SwaggerResponse(200, "FactorClientEuropeBu trouvé")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Liste FactorClientEuropeBu non trouvée")]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IReadOnlyList<FactorClientEuropeBuDto>>> GetListAsync()
    {
        var result = await _service.GetListAsync();
        
        if (!result.Any())
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Permet d'obtenir une liste d'objets FactorClientEuropeBu à partir d'un identifiant client
    /// </summary>
    /// <param name="clientId">Identifiant client de FactorClientEuropeBu</param>
    /// <returns>Liste d'objets FactorClientEuropeBu</returns>
    [SwaggerResponse(200, "Liste de FactorClientEuropeBu trouvée", typeof(FactorClientEuropeBuDto))]
    [SwaggerResponse(204, "Aucune FactorClientEuropeBu trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client FactorClientEuropeBu non trouvé")]
    [HttpGet]
    [Route("client/{clientId:int}")]
    public async Task<ActionResult<IReadOnlyList<FactorClientEuropeBuDto>>> GetByClientIdAsync([FromRoute] int clientId)
    {
        var result = await _service.GetByClientIdAsync(clientId);
        
        if (result == null || !result.Any())
        {
            return NoContent();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Permet d'obtenir une liste d'objets FactorClientEuropeBu à partir d'un identifiant BU
    /// </summary>
    /// <param name="buId">Identifiant BU de FactorClientEuropeBu</param>
    /// <returns>Liste d'objets FactorClientEuropeBu</returns>
    [SwaggerResponse(200, "Liste de FactorClientEuropeBu trouvée", typeof(FactorClientEuropeBuDto))]
    [SwaggerResponse(204, "Aucune FactorClientEuropeBu trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client FactorClientEuropeBu non trouvé")]
    [HttpGet]
    [Route("bu/{buId:int}")]
    public async Task<ActionResult<IReadOnlyList<FactorClientEuropeBuDto>>> GetByBuIdAsync([FromRoute] int buId)
    {
        var result = await _service.GetByBuIdAsync(buId);
        
        if (result == null || !result.Any())
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    /// <summary>
    /// Updates the BUs attached to a client, creating new relations, deleting non-existent ones, and keeping existing ones untouched.
    /// </summary>
    /// <returns>Relations FactorClientEuropeBu pour un client mises à jour</returns>
    [SwaggerResponse(200, "FactorClientEuropeBu mis à jour", typeof(IEnumerable<FactorClientEuropeBuDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "FactorClientEuropeBu non trouvée")]
    [HttpPut]
    [Route("factor-batch-update")]
    public async Task<ActionResult<IEnumerable<FactorClientEuropeBuDto>>> UpdateBatchAsync([FromBody] FactorBatchRequest request)
    {
        var result = await _service.UpdateBatchAsync(request);

        return Ok(result);
    }
}