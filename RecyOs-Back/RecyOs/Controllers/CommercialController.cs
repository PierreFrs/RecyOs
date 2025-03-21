// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialController.cs
// Created : 2024/03/26 - 15:07
// Updated : 2024/03/26 - 15:07

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Xunit.Sdk;

namespace RecyOs.Controllers;

[Route("commerciaux")]

public class CommercialController : BaseApiController
{
    private readonly ICommercialBaseService _commercialBaseService;
    private readonly IDataValidationService _validationService;
    public CommercialController(ICommercialBaseService commercialBaseService, IDataValidationService validationService)
    {
        _commercialBaseService = commercialBaseService;
        _validationService = validationService;
    }
    
    /// POST
    /// <summary>
    /// Permet de créer un objet Commercial
    /// </summary>
    /// <returns>Objet Commercial</returns>
    [SwaggerResponse(200, "Catégorie client créée", typeof(CommercialDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Non trouvé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "CreateCommercial")]
    public async Task<IActionResult> CreateCommercial([FromForm] CommercialDto commercialDto)
    {
        if (!_validationService.ValidatePhoneNumber(commercialDto.Phone)) return BadRequest("Invalid phone number.");
        if (!_validationService.ValidateEmailAddress(commercialDto.Email)) return BadRequest("Invalid email address.");
        
        var commercial = await _commercialBaseService.CreateAsync(commercialDto);
        if (commercial == null) return NotFound();
        return Ok(commercial);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets Commercial
    /// </summary>
    /// <returns>Liste des objets Commercial </returns>
    [SwaggerResponse(200, "Liste des objets Commercial", typeof(List<CommercialDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetList()
    {
        var commerciaux = await _commercialBaseService.GetListAsync();
        return Ok(commerciaux);
    }
    
    /// GET filtered list
    /// <summary>
    /// Permet d'obtenir la liste filtrée des objets Commercial
    /// </summary>
    /// <returns>Liste des objets Commercial </returns>
    [SwaggerResponse(200, "Liste des objets Commercial", typeof(List<CommercialDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("filtered")]
    public async Task<IActionResult> GetFilteredList([FromQuery] CommercialFilter filter)
    {
        filter = filter ?? new CommercialFilter();
        var commerciaux = await _commercialBaseService.GetFilteredListAsync(filter);
        return Ok(commerciaux);
    }
    
    /// GET by ID
    /// /// <summary>
    /// Permet d'obtenir un objet Commercial grâce à son son id
    /// </summary>
    /// <returns>Objet Commercial </returns>
    [SwaggerResponse(200, "Objet Commercial", typeof(CommercialDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Commercial non trouvée")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var commercial = await _commercialBaseService.GetByIdAsync(id);
        if (commercial == null) return NotFound();
        return Ok(commercial);
    }
    
    #nullable enable
    /// Get client by commercialId
    /// <summary>
    /// Permet d'obtenir une liste d'établissements clients filtrée en fonction de l'identifiant du commercial.
    /// </summary>
    /// <returns>Liste des établissements clients</returns>
    [SwaggerResponse(200, "Liste des établissements clients", typeof(GridData<object>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("{commercialId:int}/clients")]
    [Authorize(Policy = "ReadClient")]
    public async Task<IActionResult> GetClientsByCommercialId([FromRoute] int commercialId, [FromQuery] ClientByCommercialFilter? filter = null)
    {
        filter = filter ?? new ClientByCommercialFilter();
        var clients = await _commercialBaseService.GetClientsByCommercialIdAsync(commercialId, filter);
        if (clients == null) return NotFound();
        return Ok(clients);
    }
    #nullable disable
    
    /// <summary>
    /// Permet d'éditer un objet Commercial à partir de son identifiant
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commercialDto"></param>
    /// <returns></returns>
    [SwaggerResponse(200, "Catégorie modifiée", typeof(CommercialDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Catégorie non trouvée")]
    [SwaggerResponse(400, "Bad request : le label ne peut être nul")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "UpdateCommercial")]
    public async Task<ActionResult> UpdateCommercial([FromRoute] int id, [FromForm] CommercialDto commercialDto)
    {
        if (commercialDto == null) return BadRequest("The entity cannot be null");
        if (commercialDto.Phone != null && !_validationService.ValidatePhoneNumber(commercialDto.Phone)) return BadRequest("Invalid phone number.");
        if (commercialDto.Email != null && !_validationService.ValidateEmailAddress(commercialDto.Email)) return BadRequest("Invalid email address.");
        
        var updatedCommercialDto = await _commercialBaseService.UpdateAsync(id, commercialDto);
        if (updatedCommercialDto == null) return NotFound();
        
        return Ok(updatedCommercialDto);
    }
    
    /// DELETE
    /// <summary>
    /// Permet de supprimer un objet Commercial à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du Commercial</param>
    /// <returns></returns>
    [SwaggerResponse(200, "Commercial supprimée", typeof(CommercialDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Commercial non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "DeleteCommercial")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _commercialBaseService.DeleteAsync(id);
        if (!result) return NotFound();
        
        return Ok(result);
    }
}