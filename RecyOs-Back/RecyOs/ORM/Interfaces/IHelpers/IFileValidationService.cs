// IFileValidationService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 07/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces;

public interface IFileValidationService
{
    Task ValidateEtablissementClientId(int? etablissementClientId);
    Task ValidateEtablissementClientEuropeId(int? etablissementClientEuropeId);
    Task ValidateTypeDocumentPdfId(int? typeDocumentPdfId = null, bool isUpdate = false);
    void ValidateFile(IFormFile file);
}