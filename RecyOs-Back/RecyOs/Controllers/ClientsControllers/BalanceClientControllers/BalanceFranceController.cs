// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceFranceController.cs
// Created : 2024/02/26 - 12:14
// Updated : 2024/02/26 - 12:14

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("balances-france")]

public class BalanceFranceController : BaseApiController
{
    private readonly IBalanceFranceService _balanceFranceService;
    private readonly IEtablissementClientService _etablissementClientService;
    public BalanceFranceController(
        IBalanceFranceService balanceFranceService,
        IEtablissementClientService etablissementClientService)
    {
        _balanceFranceService = balanceFranceService;
        _etablissementClientService = etablissementClientService;
    }
    
    /// POST
    /// <summary>
    /// Permet de créer une balance Europe
    /// </summary>
    /// <returns>Un objet BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceFrance", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] BalanceDto balanceDto)
    {
        var balance = await _balanceFranceService.CreateAsync(balanceDto);
        if (balance == null)
        {
            return BadRequest();
        }
        
        return Ok(balance);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceFrance
    /// </summary>
    /// <returns>Une liste d'objets BalanceFrance</returns>
    [SwaggerResponse(200, "Liste d'objets BalanceFrance", typeof(List<BalanceDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllAsync()
    {
        var balances = await _balanceFranceService.GetAllAsync();
        return Ok(balances);
    }
    
    /// GET by ID
    /// <summary>
    /// Permet d'obtenir un objet BalanceFrance grâce à son id
    /// </summary>
    /// <returns>Un objet BalanceFrance</returns>
    [SwaggerResponse(200, "Objet BalanceFrance", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var balance = await _balanceFranceService.GetByIdAsync(id);
        if (balance == null) return NotFound();
        return Ok(balance);
    }
    
    /// GET by Client ID
    /// /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceFrance grâce à l'id du client
    /// </summary>
    /// <returns>Liste d'objets BalanceFrance</returns>
    [SwaggerResponse(200, "Objet BalanceFrance", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("client/{clientId}")]
    public async Task<IActionResult> GetByClientIdAsync(int clientId)
    {
        var client = await _etablissementClientService.GetById(clientId);
        if (client == null) return NotFound();
        var balances = await _balanceFranceService.GetByClientIdAsync(clientId);
        return Ok(balances);
    }
    
    /// PUT
    /// <summary>
    /// Permet de mettre à jour un objet BalanceFrance
    /// </summary>
    /// <returns>Un objet BalanceFrance</returns>
    [SwaggerResponse(200, "Objet BalanceFrance", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BalanceDto balanceDto)
    {
        var balance = await _balanceFranceService.UpdateAsync(id, balanceDto);
        if (balance == null) return NotFound();
        return Ok(balance);
    }
    
    /// DELETE
    /// <summary>
    /// Supprime un objet BalanceFrance
    /// </summary>
    /// <returns>Un objet BalanceFrance</returns>
    [SwaggerResponse(200, "Objet BalanceFrance", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var balance = await _balanceFranceService.DeleteAsync(id);
        if (balance)
        {
            return Ok(balance);
        }

        return NotFound();
    }

    /// GET for Grid
    /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceFrance pour une grille
    /// </summary>
    /// <returns>Liste d'objets BalanceFrance</returns>
    [SwaggerResponse(200, "Liste d'objets BalanceFrance", typeof(List<BalanceDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("grid")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] BalanceFranceGridFilter filter)
    {
        var balances = await _balanceFranceService.GetDataForGrid(filter);
        return Ok(balances);
    }
}