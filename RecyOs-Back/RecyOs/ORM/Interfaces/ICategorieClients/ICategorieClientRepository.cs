// ICategorieClientRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface ICategorieClientRepository<TCategorieClient> where TCategorieClient: CategorieClient
{
    Task<TCategorieClient> CreateCategoryAsync(TCategorieClient categorieClient);
    Task<IList<TCategorieClient>> GetListAsync(ContextSession session, bool includeDeleted = false);
    Task<TCategorieClient> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
    Task<TCategorieClient> UpdateAsync(CategorieClient categorieClient, ContextSession session);
    Task<bool> DeleteAsync(int id, ContextSession session);
}