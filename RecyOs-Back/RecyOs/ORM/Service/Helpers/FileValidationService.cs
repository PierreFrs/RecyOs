// FileValidationService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 07/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service;

public class FileValidationService : BaseService, IFileValidationService
{
    private readonly IConfiguration _configuration;
    private readonly IEtablissementClientRepository<EtablissementClient> _etablissementClientRepository;
    private readonly IClientEuropeRepository<ClientEurope> _clientEuropeRepository;
    private readonly ITypeDocumentPdfRepository<TypeDocumentPdf> _typeDocumentPdfRepository;

    public FileValidationService(
        ICurrentContextProvider contextProvider, 
        IConfiguration configuration, 
        IEtablissementClientRepository<EtablissementClient> etablissementClientRepository,
        IClientEuropeRepository<ClientEurope> clientEuropeRepository,
        ITypeDocumentPdfRepository<TypeDocumentPdf> typeDocumentPdfRepository
        ) : base(contextProvider)
    {
        _configuration = configuration;
        _etablissementClientRepository = etablissementClientRepository;
        _clientEuropeRepository = clientEuropeRepository;
        _typeDocumentPdfRepository = typeDocumentPdfRepository;
    }

    public async Task ValidateEtablissementClientId(int? etablissementClientId)
    {
        
        if (etablissementClientId <= 0)
        {
            throw new ArgumentNullException(nameof(etablissementClientId), "Etablissement Client ID cannot be null");
        }

        if (etablissementClientId > 0)
        {
            var existingEtablissementClient = await _etablissementClientRepository.Get(etablissementClientId.Value, Session);
                    if (existingEtablissementClient == null)
                    {
                        throw new EntityNotFoundException("Etablissement Client with the specified ID does not exist");
                    }
        }
    }
    
    public async Task ValidateEtablissementClientEuropeId(int? etablissementClientEuropeId)
    {
        
        if (etablissementClientEuropeId <= 0)
        {
            throw new ArgumentNullException(nameof(etablissementClientEuropeId), "Etablissement Client ID cannot be null");
        }

        if (etablissementClientEuropeId > 0)
        {
            var existingEtablissementClientEurope = await _clientEuropeRepository.GetById(etablissementClientEuropeId.Value, Session);
            if (existingEtablissementClientEurope == null)
            {
                throw new EntityNotFoundException("Etablissement Client with the specified ID does not exist");
            }
        }
    }

    public async Task ValidateTypeDocumentPdfId(int? typeDocumentPdfId = null, bool isUpdate = false)
    {
        if (typeDocumentPdfId.HasValue)
        {
            if (typeDocumentPdfId <= 0 && !isUpdate)
            {
                throw new ArgumentNullException(nameof(typeDocumentPdfId), "Type Document PDF ID cannot be null or zero for Create operation");
            }
            var existingType = await _typeDocumentPdfRepository.GetByIdAsync(typeDocumentPdfId.Value, Session);
            if (existingType == null && typeDocumentPdfId > 0)
            {
                throw new KeyNotFoundException("Type Document PDF with the specified ID does not exist");
            }
        }
        else if (!isUpdate)
        {
            throw new ArgumentNullException(nameof(typeDocumentPdfId), "Type Document PDF ID cannot be null for Create operation");
        }
    }

    public void ValidateFile(IFormFile file)
    {
        string allowedExtensions = _configuration["FileValidation:AllowedExtensions"];
        long fileSizeLimit = long.Parse(_configuration["FileValidation:FileSizeLimit"]);
        double fileSizeLimitMb = fileSizeLimit / (1024.0 * 1024.0);

        if (file.Length > fileSizeLimit)
        {
            throw new ArgumentException($"File size exceeds the maximum limit of {fileSizeLimitMb}MB");
        }

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
        {
            throw new ArgumentException($"Invalid file extension. Only {allowedExtensions} files are allowed");
        }
    }
}
