// DocumentPdfController.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[ApiController]
[Route("documents-pdf")]

public class DocumentPdfController : BaseApiController
{
    private readonly IDocumentPdfService<DocumentPdfDto> _documentPdfService;
    private readonly IFileValidationService _fileValidationService;
    private readonly IConfiguration _configuration;

    public DocumentPdfController(IDocumentPdfService<DocumentPdfDto> documentPdfService, IFileValidationService fileValidationService, ICurrentContextProvider currentContextProvider, IConfiguration configuration)
    {
        _documentPdfService = documentPdfService;
        _fileValidationService = fileValidationService;
        _configuration = configuration;
    }
    
    /// POST (Upload)
    /// <summary>
    /// Permet de créer un nouveau document PDF et de l'uploader sur le serveur
    /// </summary>
    /// <param name="file">Fichier pdf</param>
    /// <param name="etablissementClientId">Identifiant de la fiche client</param>
    /// <param name="typeDocumentPdfId">Identifiant du type de document</param>
    /// <returns>Objet crée</returns>
    [SwaggerResponse(200, "Document créé", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(400, "Bad Request - Le fichier n'a pas la bonne extension, est trop lourd ou les ID client ou type sont nuls")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "AddPdf")]
    public async Task<ActionResult> UploadDocumentPdf(IFormFile file, [FromForm] int etablissementClientId, [FromForm] int typeDocumentPdfId)
    {
            if (file == null || file.Length == 0)
                return Content("file not selected");
            
            _fileValidationService.ValidateFile(file);
            try
            {
                await _fileValidationService.ValidateEtablissementClientId(etablissementClientId);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            await _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId);
            
            var documentDto = await _documentPdfService.UploadPdfAsync(file, etablissementClientId, typeDocumentPdfId);
            return Ok(documentDto);
    }

    /// GET by ID (Download)
    /// /// <summary>
    /// Permet de télécharger un objet DocumentPdf grâce à son son ID
    /// </summary>
    /// <returns>Objet DocumentPdf</returns>
    [SwaggerResponse(200, "Objet DocumentPdf", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpGet]
    [Route("download/{id:int}")]
    [Authorize(Policy = "DownloadPdf")]
    public async Task<ActionResult> DownloadDocumentPdf(int id)
    {
        string mimeTypeConfig = _configuration["FileValidation:MimeType"];

        var (fileStream, fullPath) = await _documentPdfService.DownloadPdfAsync(id);
        if (fileStream == null || fullPath == null)
        {
            return NotFound();
        }

        var mimeType = mimeTypeConfig;
        return File(fileStream, mimeType, Path.GetFileName(fullPath));
    }
    
    
    /// GET
    /// <summary>
    /// Permet d'obtenir la liste des objets DocumentPdf
    /// </summary>
    /// <returns>Liste des objets DocumentPdf </returns>
    [SwaggerResponse(200, "Liste des objets DocumentPdf", typeof(List<DocumentPdfDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetList()
    {
        var documents = await _documentPdfService.GetListAsync();
        return Ok(documents);
    }
    
    /// GET by ID
    /// /// <summary>
    /// Permet d'obtenir un objet DocumentPdf grâce à son son id
    /// </summary>
    /// <returns>Objet DocumentPdf </returns>
    [SwaggerResponse(200, "Objet DocumentPdf", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var document = await _documentPdfService.GetByIdAsync(id);
        if (document == null) return NotFound();
        return Ok(document);
    }
    
    /// GET by Client ID
    /// /// <summary>
    /// Permet d'obtenir une liste d'objets DocumentPdf grâce à l'ID client
    /// </summary>
    /// <returns>Liste DocumentPdf par ID client</returns>
    [SwaggerResponse(200, "Objet DocumentPdf", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpGet]
    [Route("client/{clientId:int}")]
    public async Task<IActionResult> GetByClientId(int clientId)
    {
        var documents = await _documentPdfService.GetByClientIdAsync(clientId);
        return Ok(documents);
    }
    
    /// PUT
    /// <summary>
    /// Permet d'éditer un objet DocumentPdf à partir de son identifiant
    /// </summary>
    /// <param name="file">Fichier pdf</param>
    /// <param name="id">Identifiant du DocumentPdf</param>
    /// <param name="typeDocumentPdfId">Identifiant du type de document</param>
    /// <returns>Objet DocumentPdf</returns>
    [SwaggerResponse(200, "Document modifié", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policy = "UpdatePdf")]
    public async Task<ActionResult> UpdateDocumentPdf(IFormFile file, [FromRoute] int id, [FromForm] int? typeDocumentPdfId = null)
    {
        if (file != null)
        {
            _fileValidationService.ValidateFile(file);
            await _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, true);
        }
            
        int? typeIdToPass = typeDocumentPdfId.HasValue ? typeDocumentPdfId.Value : null;
    
        var documentDto = await _documentPdfService.UpdatePdfAsync(id, file, typeIdToPass);
        return Ok(documentDto);
    }
    
    /// DELETE
    /// <summary>
    /// Permet de supprimer un objet DocumentPdf à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du DocumentPdf</param>
    /// <returns></returns>
    [SwaggerResponse(200, "Document supprimé", typeof(DocumentPdfDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Document non trouvé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "DeletePdf")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _documentPdfService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok(result);
    }
}