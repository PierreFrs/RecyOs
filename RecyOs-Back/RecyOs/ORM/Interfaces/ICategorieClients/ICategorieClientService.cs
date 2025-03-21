// ICategorieClientService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyOs.ORM.Interfaces;

public interface ICategorieClientService<TCategoreCLientDto>
{
    Task<TCategoreCLientDto> CreateCategoryAsync(string label);
    Task<List<TCategoreCLientDto>> GetListAsync();
    Task<TCategoreCLientDto> GetByIdAsync(int id);
    Task<TCategoreCLientDto> UpdateCategorieClientAsync(int id, string label);
    Task<bool> DeleteAsync(int id);
}