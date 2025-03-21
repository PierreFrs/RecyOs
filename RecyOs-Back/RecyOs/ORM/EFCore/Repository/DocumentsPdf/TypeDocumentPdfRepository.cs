// TypeDocumentPdfRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository
{
    public class TypeDocumentPdfRepository : BaseDeletableRepository<TypeDocumentPdf, DataContext>, ITypeDocumentPdfRepository<TypeDocumentPdf>
    {
        private readonly DataContext _context;

        public TypeDocumentPdfRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TypeDocumentPdf> CreateTypeAsync(TypeDocumentPdf typeDocumentPdf)
        {
            _context.Add(typeDocumentPdf);
            await _context.SaveChangesAsync();

            return typeDocumentPdf;
        }

        public async Task<IList<TypeDocumentPdf>> GetListAsync(ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .ToListAsync();
        }
        public async Task<TypeDocumentPdf> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Where(obj => obj.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TypeDocumentPdf> GetByLabelAsync(string label, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Where(obj => obj.Label == label)
                .FirstOrDefaultAsync();
        }

        public async Task<TypeDocumentPdf> UpdateAsync(TypeDocumentPdf typeDocumentPdf, ContextSession session)
        {
            var context = GetContext(session);

            var existingTypeDocumentPdf = await context.TypeDocumentPdfs.FindAsync(typeDocumentPdf.Id);
            if (existingTypeDocumentPdf == null)
            {
                return null;
            }

            existingTypeDocumentPdf.Label = typeDocumentPdf.Label;
            existingTypeDocumentPdf.UpdatedAt = typeDocumentPdf.UpdatedAt;
            existingTypeDocumentPdf.UpdatedBy = typeDocumentPdf.UpdatedBy;

            context.Entry(existingTypeDocumentPdf).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return existingTypeDocumentPdf;
        }

        public async Task<bool> DeleteAsync(int id, ContextSession session)
        {
            await Delete(id, session);
            return true;
        }
    }
}