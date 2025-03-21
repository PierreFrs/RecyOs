// ITypeDocumentPdfService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;

namespace RecyOs.ORM.Interfaces;

public interface ITypeDocumentPdfService<TTypeDocumentPdfDto>
{
    Task<TTypeDocumentPdfDto> CreateTypeAsync(string label);
    Task<List<TTypeDocumentPdfDto>> GetAllAsync();
    Task<TTypeDocumentPdfDto> GetByIdAsync(int id);
    Task<TTypeDocumentPdfDto> GetByLabelAsync(string label);
    Task<TTypeDocumentPdfDto> UpdateAsync(int id, string label);
    Task<bool> DeleteAsync(int id);
    
}