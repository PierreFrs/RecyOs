// IDocumentPdfRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 04/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces
{
    public interface IDocumentPdfEuropeRepository<TDocumentPdfEurope> where TDocumentPdfEurope : DocumentPdfEurope, new()
    {
        Task<TDocumentPdfEurope> CreateAsync(DocumentPdfEurope documentPdfEurope);
        Task<IList<TDocumentPdfEurope>> GetListAsync(ContextSession session, bool includeDeleted = false);
        Task<TDocumentPdfEurope> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
        Task<IList<TDocumentPdfEurope>> GetByClientIdAsync(int clientEuropeId, ContextSession session, bool includeDeleted = false);
        Task<TDocumentPdfEurope> UpdateAsync(DocumentPdfEurope documentPdfEurope, ContextSession session);
        Task<bool> DeleteAsync(int id, ContextSession session);
        
    }
}