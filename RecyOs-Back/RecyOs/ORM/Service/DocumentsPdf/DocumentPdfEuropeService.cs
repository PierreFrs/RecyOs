// DocumentPdfService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class DocumentPdfEuropeService<TDocumentPdfEurope, TDocumentPdfEuropeDto> : BaseService, IDocumentPdfEuropeService<TDocumentPdfEuropeDto> 
    where TDocumentPdfEurope : DocumentPdfEurope, new()
    where TDocumentPdfEuropeDto : DocumentPdfEuropeDto, new()
{
    private readonly IDocumentPdfEuropeRepository<TDocumentPdfEurope> _documentPdfEuropeRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ITokenInfoService _tokenInfoService;
    private readonly IFileSystem _fileSystem;

    public DocumentPdfEuropeService(ICurrentContextProvider contextProvider, IDocumentPdfEuropeRepository<TDocumentPdfEurope> documentPdfEuropeRepository, IMapper mapper, IConfiguration configuration, ITokenInfoService tokenInfoService, IFileSystem fileSystem) : base(contextProvider)
    {
        _documentPdfEuropeRepository = documentPdfEuropeRepository;
        _mapper = mapper;
        _configuration = configuration;
        _tokenInfoService = tokenInfoService;
        _fileSystem = fileSystem;
    }

    public async Task<TDocumentPdfEuropeDto> UploadPdfAsync(IFormFile file, int etablissementClientEuropeId, int typeDocumentPdfId)
    {
        var pdfFolder = _configuration["FileRepository:PdfFolderEurope"];

        var currentUser = _tokenInfoService.GetCurrentUserName();
        
        // Handling file name => concatanating of : guid + original file name + extension
        var guid = Guid.NewGuid().ToString();
        var sanitizedFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{guid}_{sanitizedFileName}{extension}";
        
        // Handling file path => relative file path + file name
        if (!_fileSystem.DirectoryExists(pdfFolder)) _fileSystem.CreateDirectory(pdfFolder);
        var fullPath = Path.Combine(pdfFolder, fileName);
        
        // Copy the file via stream
        using (var stream = _fileSystem.CreateFileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        // Create new DTO, convert to an entity, update to database, convert it back to a dto and returns it for client response
        var documentPdfEuropeDto = new DocumentPdfEuropeDto
        {
            FileName = fileName,
            FileSize = (int)file.Length,
            FileLocation = fullPath,
            EtablissementClientEuropeID = etablissementClientEuropeId,
            TypeDocumentPdfId = typeDocumentPdfId,
            CreateDate = DateTime.Now,
            CreatedBy = currentUser,
        };
        var documentEntity = _mapper.Map<TDocumentPdfEurope>(documentPdfEuropeDto);
        var savedDocumentEntity = await _documentPdfEuropeRepository.CreateAsync(documentEntity);
        var savedDocumentDto = _mapper.Map<TDocumentPdfEuropeDto>(savedDocumentEntity);

        return savedDocumentDto;
    }
    
    public async Task<(FileStream FileStream, string FullPath)> DownloadPdfAsync(int id)
    {
        var documentEntity = await _documentPdfEuropeRepository.GetByIdAsync(id, Session);
        if (documentEntity == null) return (null, null);
        
        if (!_fileSystem.FileExists(documentEntity.FileLocation)) return (null, null);
        
        var fileStream = new FileStream(documentEntity.FileLocation, FileMode.Open, FileAccess.Read);
        var originalFileName = documentEntity.FileName.Substring(documentEntity.FileName.IndexOf('_') + 1);
        return (fileStream, originalFileName);
    }
    
    public async Task<List<TDocumentPdfEuropeDto>> GetListAsync()
    {
        var documents = await _documentPdfEuropeRepository.GetListAsync(Session);
        return _mapper.Map<List<TDocumentPdfEuropeDto>>(documents);
    }

    public async Task<TDocumentPdfEuropeDto> GetByIdAsync(int id)
    {
        var document = await _documentPdfEuropeRepository.GetByIdAsync(id, Session);
        return _mapper.Map<TDocumentPdfEuropeDto>(document);
    }

    public async Task<List<TDocumentPdfEuropeDto>> GetByClientIdAsync(int clientEuropeId)
    {
        var documents = await _documentPdfEuropeRepository.GetByClientIdAsync(clientEuropeId, Session);
        return _mapper.Map<List<TDocumentPdfEuropeDto>>(documents);
    }

    public async Task<TDocumentPdfEuropeDto> UpdatePdfAsync(int id, IFormFile file, int? typeDocumentPdfId)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();
    
        var existingDocument = await _documentPdfEuropeRepository.GetByIdAsync(id, Session);
        if (existingDocument == null) return null;
    
        if (file != null)
        {
            var oldFilePath = existingDocument.FileLocation;
            if (_fileSystem.FileExists(oldFilePath)) _fileSystem.DeleteFile(oldFilePath);
        
            var guid = Guid.NewGuid().ToString();
            var sanitizedFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{guid}_{sanitizedFileName}{extension}";

            var pdfFolder = _configuration["FileRepository:PdfFolderEurope"];
            if (!_fileSystem.DirectoryExists(pdfFolder)) _fileSystem.CreateDirectory(pdfFolder);
            var fullPath = Path.Combine(pdfFolder, fileName);

            using (var stream = _fileSystem.CreateFileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            existingDocument.FileName = fileName;
            existingDocument.FileSize = (int)file.Length;
            existingDocument.FileLocation = fullPath;
            existingDocument.UpdatedAt = DateTime.Now;
            existingDocument.UpdatedBy = currentUser;
        }
        
    if (typeDocumentPdfId.HasValue && typeDocumentPdfId > 0) existingDocument.TypeDocumentPdfId = typeDocumentPdfId.Value;
    
    var updatedDocumentEntity = await _documentPdfEuropeRepository.UpdateAsync(existingDocument, Session);
    var updatedDocumentDto = _mapper.Map<TDocumentPdfEuropeDto>(updatedDocumentEntity);

    return updatedDocumentDto;
}

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _documentPdfEuropeRepository.GetByIdAsync(id, Session);
        if (entity == null)
        {
            return false;
        }
        await _documentPdfEuropeRepository.DeleteAsync(id, Session);
        return true;
    }

    
}
