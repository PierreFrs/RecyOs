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

public interface IDocumentPdfService<TDocumentPdfDto>
{
    Task<TDocumentPdfDto> UploadPdfAsync(IFormFile file, int etablissementClientId, int typeDocumentPdfId);
    Task<(FileStream FileStream, string FullPath)> DownloadPdfAsync(int id);
    Task<List<TDocumentPdfDto>> GetListAsync();
    Task<TDocumentPdfDto> GetByIdAsync(int id);
    Task<List<TDocumentPdfDto>> GetByClientIdAsync(int clientId);
    Task<TDocumentPdfDto> UpdatePdfAsync(int id, IFormFile file, int? typeDocumentPdfId = 0);
    Task<bool> DeleteAsync(int id);
}