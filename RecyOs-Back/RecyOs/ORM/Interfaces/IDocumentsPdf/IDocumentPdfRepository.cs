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
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces
{
    public interface IDocumentPdfRepository<TDocumentPdf> where TDocumentPdf : DocumentPdf, new()
    {
        Task<TDocumentPdf> CreateAsync(TDocumentPdf documentPdf);
        Task<IList<TDocumentPdf>> GetListAsync(ContextSession session, bool includeDeleted = false);
        Task<TDocumentPdf> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
        Task<IList<TDocumentPdf>> GetByClientIdAsync(int clientId, ContextSession session, bool includeDeleted = false);
        Task<TDocumentPdf> UpdateAsync(TDocumentPdf documentPdf, ContextSession session);
        Task<ServiceResult> UpdateClientIdInDocumentPdfsAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session);
        Task<bool> DeleteAsync(int id, ContextSession session);
        
    }
}