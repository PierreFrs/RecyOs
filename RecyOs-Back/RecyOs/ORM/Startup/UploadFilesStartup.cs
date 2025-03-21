// UploadFilesStartup.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class UploadFilesStartup
{
    public static void ConfigureFilesUpload(IServiceCollection services)
    {
        services.AddTransient<IDocumentPdfRepository<DocumentPdf>, DocumentPdfRepository>();
        services.AddTransient<IDocumentPdfService<DocumentPdfDto>, DocumentPdfService<DocumentPdf, DocumentPdfDto>>();
        
        services.AddTransient<IDocumentPdfEuropeRepository<DocumentPdfEurope>, DocumentPdfEuropeRepository>();
        services.AddTransient<IDocumentPdfEuropeService<DocumentPdfEuropeDto>, DocumentPdfEuropeService<DocumentPdfEurope, DocumentPdfEuropeDto>>();
        
        services.AddTransient<ITypeDocumentPdfRepository<TypeDocumentPdf>, TypeDocumentPdfRepository>();
        services.AddTransient<ITypeDocumentPdfService<TypeDocumentPdfDto>, TypeDocumentPdfService<TypeDocumentPdf, TypeDocumentPdfDto>>();
        
        services.AddTransient<IFileValidationService, FileValidationService>();

        services.AddTransient<IFileSystem, FileSystem>();
    }
}