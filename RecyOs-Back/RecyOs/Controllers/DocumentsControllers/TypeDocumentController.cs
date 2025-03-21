// TypeDocumentController.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("documents-types")]

public class TypeDocumentController : BaseApiController
{
    private readonly ITypeDocumentPdfService<TypeDocumentPdfDto> _typeDocumentPdfService;

    public TypeDocumentController(ITypeDocumentPdfService<TypeDocumentPdfDto> typeDocumentPdfService)
    {
        _typeDocumentPdfService = typeDocumentPdfService;
    }
    
    /// POST
    /// <summary>
    /// Permet de créer un objet TypeDocumentPdf
    /// </summary>
    /// <returns>Objet TypeDocumentPdf</returns>
    [SwaggerResponse(200, "Type de document créé", typeof(TypeDocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateType([FromForm]string typeLabel)
    {
        var type = await _typeDocumentPdfService.CreateTypeAsync(typeLabel);
        if (type == null) return NotFound();
        return Ok(type);
    }
    
    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets TypeDocumentPdf
    /// </summary>
    /// <returns>Liste des objets TypeDocumentPdf </returns>
    [SwaggerResponse(200, "Liste des objets TypeDocumentPdf", typeof(List<TypeDocumentPdfDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var types = await _typeDocumentPdfService.GetAllAsync();
        return Ok(types);
    }
    
    /// GET by Id
    /// <summary>
    /// Permet d'obtenir un objet TypeDocumentPdf à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du TypeDocumentPdf</param>
    /// <returns>Objet TypeDocumentPdf</returns>
    [SwaggerResponse(200, "Type de document trouvé", typeof(TypeDocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Type non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var type = await _typeDocumentPdfService.GetByIdAsync(id);
        if (type == null) return NotFound();
        return Ok(type);
    }
    
    /// GET by Label
    /// <summary>
    /// Permet d'obtenir un objet TypeDocumentPdf à partir de son label
    /// </summary>
    /// <param name="label">Identifiant du TypeDocumentPdf</param>
    /// <returns>Objet TypeDocumentPdf</returns>
    [SwaggerResponse(200, "Type de document trouvé", typeof(TypeDocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Type non trouvé")]
    [HttpGet]
    [Route("{label}")]
    public async Task<IActionResult> GetByLabel(string label)
    {
        var type = await _typeDocumentPdfService.GetByLabelAsync(label);
        if (type == null) return NotFound();
        return Ok(type);
    }

    
    /// PUT
    /// <summary>
    /// Permet d'éditer un objet TypeDocumentPdf à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du TypeDocumentPdf</param>
    /// <param name="label">Label du TypeDocumentPdf</param>
    /// <returns>Objet TypeDocumentPdf</returns>
    [SwaggerResponse(200, "Type de document modifié", typeof(TypeDocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Type non trouvé")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] string label)
    {
        
        var type = await _typeDocumentPdfService.UpdateAsync(id, label);
        if (type == null) return NotFound();
        return Ok(type);
    }
    
    /// DELETE
    /// <summary>
    /// Permet de supprimer un objet TypeDocumentPdf à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du TypeDocumentPdf</param>
    /// <returns></returns>
    [SwaggerResponse(200, "Type de document supprimé", typeof(TypeDocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Type non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var type = await _typeDocumentPdfService.DeleteAsync(id);
        if (!type) return NotFound();
        return Ok(type);
    }
}