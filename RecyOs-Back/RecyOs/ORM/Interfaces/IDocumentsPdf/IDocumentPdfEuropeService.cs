// IDocumentPdfService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IDocumentPdfEuropeService<TDocumentPdfEuropeDto>
{
    Task<TDocumentPdfEuropeDto> UploadPdfAsync(IFormFile file, int etablissementClientEuropeId, int typeDocumentPdfId);
    Task<(FileStream FileStream, string FullPath)> DownloadPdfAsync(int id);
    Task<List<TDocumentPdfEuropeDto>> GetListAsync();
    Task<TDocumentPdfEuropeDto> GetByIdAsync(int id);
    Task<List<TDocumentPdfEuropeDto>> GetByClientIdAsync(int clientEuropeId);
    Task<TDocumentPdfEuropeDto> UpdatePdfAsync(int id, IFormFile file, int? typeDocumentPdfId = 0);
    Task<bool> DeleteAsync(int id);


    
}