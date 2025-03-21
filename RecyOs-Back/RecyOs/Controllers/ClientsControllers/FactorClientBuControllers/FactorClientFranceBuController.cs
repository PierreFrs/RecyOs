// <copyright file="FactorClientFranceBuController.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;
[Route("factor_client_france_bu")]

public class FactorClientFranceBuController : BaseApiController
{
    private readonly IFactorClientFranceBuService _service;

    public FactorClientFranceBuController(IFactorClientFranceBuService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Permet d'obtenir la liste de tous les objets FactorClientFranceBu
    /// </summary>
    /// <returns>Liste d'objets FactorClientFranceBu</returns>
    [SwaggerResponse(200, "FactorClientFranceBu trouvé")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Liste FactorClientFranceBu non trouvée")]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IReadOnlyList<FactorClientFranceBuDto>>> GetListAsync()
    {
        var result = await _service.GetListAsync();
        
        if (!result.Any())
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Permet d'obtenir une liste d'objets FactorClientFranceBu à partir d'un identifiant client
    /// </summary>
    /// <param name="clientId">Identifiant client de FactorClientFranceBu</param>
    /// <returns>Liste d'objets FactorClientFranceBu</returns>
    [SwaggerResponse(200, "Liste de FactorClientFranceBu trouvée", typeof(FactorClientFranceBuDto))]
    [SwaggerResponse(204, "Aucune FactorClientEuropeBu trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client FactorClientFranceBu non trouvé")]
    [HttpGet]
    [Route("client/{clientId:int}")]
    public async Task<ActionResult<IReadOnlyList<FactorClientFranceBuDto>>> GetByClientIdAsync([FromRoute] int clientId)
    {
        var result = await _service.GetByClientIdAsync(clientId);
        
        if (result == null || !result.Any())
        {
            return NoContent();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Permet d'obtenir une liste d'objets FactorClientFranceBu à partir d'un identifiant BU
    /// </summary>
    /// <param name="buId">Identifiant BU de FactorClientFranceBu</param>
    /// <returns>Liste d'objets FactorClientFranceBu</returns>
    [SwaggerResponse(200, "Liste de FactorClientFranceBu trouvée", typeof(FactorClientFranceBuDto))]
    [SwaggerResponse(204, "Aucune FactorClientEuropeBu trouvée")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client FactorClientFranceBu non trouvé")]
    [HttpGet]
    [Route("bu/{buId:int}")]
    public async Task<ActionResult<IReadOnlyList<FactorClientFranceBuDto>>> GetByBuIdAsync([FromRoute] int buId)
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
    /// <returns>Relations FactorClientFranceBu pour un client mises à jour</returns>
    [SwaggerResponse(200, "FactorClientFranceBu mis à jour", typeof(IEnumerable<FactorClientFranceBuDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "FactorClientFranceBu non trouvée")]
    [HttpPut]
    [Route("factor-batch-update")]
    public async Task<ActionResult<IEnumerable<FactorClientFranceBuDto>>> UpdateBatchAsync([FromBody] FactorBatchRequest request)
    {
        var result = await _service.UpdateBatchAsync(request);
        
        return Ok(result);
    }
}