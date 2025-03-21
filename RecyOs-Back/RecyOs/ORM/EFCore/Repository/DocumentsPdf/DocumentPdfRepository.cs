// DocumentPdfRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 04/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository;

public class DocumentPdfRepository :  BaseDeletableRepository<DocumentPdf, DataContext>, IDocumentPdfRepository<DocumentPdf>
{
    private readonly DataContext _context;
    private readonly ITokenInfoService _tokenInfoService;
    public DocumentPdfRepository(
        DataContext context,
        ITokenInfoService tokenInfoService
        ) : base(context)
    {
        _context = context;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<DocumentPdf> CreateAsync(DocumentPdf documentPdf)
    {
        _context.Add(documentPdf);
        await _context.SaveChangesAsync();

        return documentPdf;
    }
    
    public async Task<IList<DocumentPdf>> GetListAsync(ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Include(x => x.TypeDocumentPdf)
            .ToListAsync();
    }

    public async Task<DocumentPdf> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .Include(x => x.TypeDocumentPdf)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IList<DocumentPdf>> GetByClientIdAsync(int clientId, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.EtablissementClientId == clientId)
            .Include(x => x.TypeDocumentPdf)
            .ToListAsync();
    }

    public async Task<DocumentPdf> UpdateAsync(DocumentPdf documentPdf, ContextSession session)
    {
        var context = GetContext(session);

        // Check if object exists in database
        var existingDocumentPdf = await context.DocumentPdfs.FindAsync(documentPdf.Id);
        if (existingDocumentPdf == null)
        {
            // Handle the scenario where the object doesn't exist, perhaps throw an exception or return null
            return null;
        }

        // Update fields of existing entity
        existingDocumentPdf.FileName = documentPdf.FileName;
        existingDocumentPdf.FileSize = documentPdf.FileSize;
        existingDocumentPdf.FileLocation = documentPdf.FileLocation;
        existingDocumentPdf.TypeDocumentPdfId = documentPdf.TypeDocumentPdfId;
        existingDocumentPdf.UpdatedAt = documentPdf.UpdatedAt;
        existingDocumentPdf.UpdatedBy = documentPdf.UpdatedBy;

        context.Entry(existingDocumentPdf).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return existingDocumentPdf;
    }

    public async Task<ServiceResult> UpdateClientIdInDocumentPdfsAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session)
    {
        try
        {
            // Fetch the list of DocumentPdf entities associated with the old client ID
            var docList = await _context.DocumentPdfs
                .Where(d => d.EtablissementClientId == oldEtablissementClientId)
                .ToListAsync();
            
            // Mark the old entities as deleted
            foreach (var doc in docList)
            {
                doc.IsDeleted = true;
                doc.UpdatedAt = DateTime.Now;
                doc.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            }
            
            // Create and add new entities
            foreach (var doc in docList)
            {
                var newDoc = new DocumentPdf
                {
                    FileSize = doc.FileSize,
                    FileName = doc.FileName,
                    FileLocation = doc.FileLocation,
                    TypeDocumentPdfId = doc.TypeDocumentPdfId,
                    EtablissementClientId = newEtablissementId,
                    CreatedBy = doc.CreatedBy,
                    CreateDate = doc.CreateDate,
                    UpdatedBy = _tokenInfoService.GetCurrentUserName(),
                    UpdatedAt = DateTime.Now
                };
                await _context.DocumentPdfs.AddAsync(newDoc);
            }
            
            await _context.SaveChangesAsync();
        
            // Verify the changes
            var verificationList = await _context.DocumentPdfs
                .Where(d => d.EtablissementClientId == newEtablissementId)
                .AsNoTracking()
                .ToListAsync();
            
            if (verificationList.Count != docList.Count)
            {
                return new ServiceResult
                {
                    Success = false, 
                    StatusCode = 400,
                    Message = "Une erreur est survenue lors du transfert des documents"
                };
            
            }
            else
            {
                return new ServiceResult
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Les documents ont bien été transférés"
                };
            }
        }
        catch (Exception e)
        {
            return new ServiceResult
            {
                Success = false,
                StatusCode = 500,
                Message = e.Message
            };
        }
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        await Delete(id, session);
        return true;
    }
}