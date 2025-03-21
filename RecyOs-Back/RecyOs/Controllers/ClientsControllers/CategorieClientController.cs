// CategorieClientController.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("categories-clients")]

public class CategorieClientController : BaseApiController
{
    private readonly ICategorieClientService<CategorieClientDto> _categorieClientService;

    public CategorieClientController(ICategorieClientService<CategorieClientDto> categorieClientService)
    {
        _categorieClientService = categorieClientService;
    }
    
    /// POST
    /// <summary>
    /// Permet de créer un objet CategorieClient
    /// </summary>
    /// <returns>Objet CategorieClient</returns>
    [SwaggerResponse(200, "Catégorie client créée", typeof(CategorieClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Non trouvé")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateCategory([FromForm] string categorieLabel)
    {
        var category = await _categorieClientService.CreateCategoryAsync(categorieLabel);
        if (category == null) return NotFound();
        return Ok(category);
    }

    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets CategorieClient
    /// </summary>
    /// <returns>Liste des objets CategorieClient </returns>
    [SwaggerResponse(200, "Liste des objets CategorieClient", typeof(List<CategorieClientDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetList()
    {
        var categories = await _categorieClientService.GetListAsync();
        return Ok(categories);
    }
    
    /// GET by ID
    /// /// <summary>
    /// Permet d'obtenir un objet CategorieClient grâce à son son id
    /// </summary>
    /// <returns>Objet CategorieClient </returns>
    [SwaggerResponse(200, "Objet CategorieClient", typeof(CategorieClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Catégorie non trouvée")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categorieClientService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }
    
    /// PUT
    /// <summary>
    /// Permet d'éditer un objet CategorieClient à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du CategorieClient</param>
    /// <param name="label">Label</param>
    /// <returns>Objet CategorieClient</returns>
    [SwaggerResponse(200, "Catégorie modifiée", typeof(CategorieClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Catégorie non trouvée")]
    [SwaggerResponse(400, "Bad request : le label ne peut être nul")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult> UpdateCategorieClient([FromRoute] int id, [FromForm] string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return BadRequest("The label cannot be empty");
        }
        var categorieClientDto = await _categorieClientService.UpdateCategorieClientAsync(id, label);
        if (categorieClientDto == null)
        {
            return NotFound();
        }
        return Ok(categorieClientDto);
    }
    
    /// DELETE
    /// <summary>
    /// Permet de supprimer un objet CategorieClient à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du CategorieClient</param>
    /// <returns></returns>
    [SwaggerResponse(200, "Catégorie supprimée", typeof(CategorieClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _categorieClientService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok(result);
    }
}