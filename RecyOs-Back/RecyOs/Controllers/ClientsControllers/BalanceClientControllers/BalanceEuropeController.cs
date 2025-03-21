// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceEuropeController.cs
// Created : 2024/02/26 - 14:40
// Updated : 2024/02/26 - 14:40

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("balances-europe")]
public class BalanceEuropeController : BaseApiController
{
    private readonly IBalanceEuropeService _balanceEuropeService;
    private readonly IClientEuropeService _clientEuropeService;
    public BalanceEuropeController(
        IBalanceEuropeService balanceEuropeService,
        IClientEuropeService clientEuropeService)
    {
        _balanceEuropeService = balanceEuropeService;
        _clientEuropeService = clientEuropeService;
    }
    
    /// POST
    /// <summary>
    /// Permet de créer une balance Europe
    /// </summary>
    /// <returns>Un objet BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceEurope", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] BalanceDto balanceDto)
    {
        var balance = await _balanceEuropeService.CreateAsync(balanceDto);
        if (balance == null)
        {
            return BadRequest();
        }
        
        return Ok(balance);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceEurope
    /// </summary>
    /// <returns>Une liste d'objets BalanceEurope</returns>
    [SwaggerResponse(200, "Liste d'objets BalanceEurope", typeof(IReadOnlyList<BalanceDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllAsync()
    {
        var balances = await _balanceEuropeService.GetAllAsync();
        return Ok(balances);
    }
    
    /// GET by ID
    /// <summary>
    /// Permet d'obtenir un objet BalanceEurope grâce à son id
    /// </summary>
    /// <returns>Un objet BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceEurope", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var balance = await _balanceEuropeService.GetByIdAsync(id);
        if (balance == null) return NotFound();
        return Ok(balance);
    }
    
    /// GET by Client ID
    /// /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceEurope grâce à l'id du client
    /// </summary>
    /// <returns>Liste d'objets BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceEurope", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("client/{clientId}")]
    public async Task<IActionResult> GetByClientIdAsync(int clientId)
    {
        var client = await _clientEuropeService.GetById(clientId);
        if (client == null) return NotFound();
        var balances = await _balanceEuropeService.GetByClientIdAsync(clientId);
        return Ok(balances);
    }
    
    
    /// PUT
    /// <summary>
    /// Permet de mettre à jour un objet BalanceEurope
    /// </summary>
    /// <returns>Un objet BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceEurope", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BalanceDto balanceDto)
    {
        var balance = await _balanceEuropeService.UpdateAsync(id, balanceDto);
        if (balance == null) return NotFound();
        return Ok(balance);
    }
    
    /// DELETE
    /// <summary>
    /// Supprime un objet BalanceEurope
    /// </summary>
    /// <returns>Un objet BalanceEurope</returns>
    [SwaggerResponse(200, "Objet BalanceEurope", typeof(BalanceDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var balance = await _balanceEuropeService.DeleteAsync(id);
        if (balance)
        {
            return Ok(balance);
        }
       
        return NotFound();
    }

    /// GET for Grid
    /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceEurope pour une grille
    /// </summary>
    /// <returns>Liste d'objets BalanceEurope</returns>
    [SwaggerResponse(200, "Liste d'objets BalanceEurope", typeof(List<BalanceEuropeDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Balance(s) non trouvée")]
    [HttpGet]
    [Route("grid")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] BalanceEuropeGridFilter filter)
    {
        var balances = await _balanceEuropeService.GetDataForGrid(filter);
        return Ok(balances);
    }
}