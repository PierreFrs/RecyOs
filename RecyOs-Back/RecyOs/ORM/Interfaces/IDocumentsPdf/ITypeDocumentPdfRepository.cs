// ITypeDocumentPdfRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface ITypeDocumentPdfRepository<TTypeDocumentPdf> where TTypeDocumentPdf : TypeDocumentPdf
{
    Task<TTypeDocumentPdf> CreateTypeAsync(TTypeDocumentPdf typeDocumentPdf);
    Task<IList<TTypeDocumentPdf>> GetListAsync(ContextSession session, bool includeDeleted = false);
    Task<TTypeDocumentPdf> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
    Task<TTypeDocumentPdf> GetByLabelAsync(string label, ContextSession session, bool includeDeleted = false);
    Task<TTypeDocumentPdf> UpdateAsync(TTypeDocumentPdf typeDocumentPdf, ContextSession session);
    Task<bool> DeleteAsync(int id, ContextSession session);
    
}