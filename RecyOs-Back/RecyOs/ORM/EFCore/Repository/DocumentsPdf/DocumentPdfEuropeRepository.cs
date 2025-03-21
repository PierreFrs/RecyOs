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

namespace RecyOs.ORM.EFCore.Repository;

public class DocumentPdfEuropeRepository :  BaseDeletableRepository<DocumentPdfEurope, DataContext>, IDocumentPdfEuropeRepository<DocumentPdfEurope>
{
    private readonly DataContext _context;
    public DocumentPdfEuropeRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<DocumentPdfEurope> CreateAsync(DocumentPdfEurope documentPdfEurope)
    {
        _context.Add(documentPdfEurope);
        await _context.SaveChangesAsync();

        return documentPdfEurope;
    }
    
    public async Task<IList<DocumentPdfEurope>> GetListAsync(ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Include(x => x.TypeDocumentPdf)
            .ToListAsync();
    }

    public async Task<DocumentPdfEurope> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .Include(x => x.TypeDocumentPdf)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IList<DocumentPdfEurope>> GetByClientIdAsync(int clientEuropeId, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.EtablissementClientEuropeId == clientEuropeId)
            .Include(x => x.TypeDocumentPdf)
            .ToListAsync();
    }

    public async Task<DocumentPdfEurope> UpdateAsync(DocumentPdfEurope documentPdfEurope, ContextSession session)
    {
        var context = GetContext(session);
        
        var existingDocumentPdf = await context.DocumentPdfEuropes.FindAsync(documentPdfEurope.Id);
        if (existingDocumentPdf == null)
        {
            return null;
        }

        // Update fields of existing entity
        existingDocumentPdf.FileName = documentPdfEurope.FileName;
        existingDocumentPdf.FileSize = documentPdfEurope.FileSize;
        existingDocumentPdf.FileLocation = documentPdfEurope.FileLocation;
        existingDocumentPdf.TypeDocumentPdfId = documentPdfEurope.TypeDocumentPdfId;
        existingDocumentPdf.UpdatedAt = documentPdfEurope.UpdatedAt;
        existingDocumentPdf.UpdatedBy = documentPdfEurope.UpdatedBy;

        context.Entry(existingDocumentPdf).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return existingDocumentPdf;
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        await Delete(id, session);
        return true;
    }

    
}