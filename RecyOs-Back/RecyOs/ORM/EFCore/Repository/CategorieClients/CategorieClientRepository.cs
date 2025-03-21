// CategorieClientRepository.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class CategorieClientRepository : BaseDeletableRepository<CategorieClient, DataContext>, ICategorieClientRepository<CategorieClient>
{
    private readonly DataContext _context;

    public CategorieClientRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CategorieClient> CreateCategoryAsync(CategorieClient categorieClient)
    {
        _context.Add(categorieClient);
        await _context.SaveChangesAsync();
        return categorieClient;
    }
    
    public async Task<IList<CategorieClient>> GetListAsync(ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .ToListAsync();
    }

    public async Task<CategorieClient> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<CategorieClient> UpdateAsync(CategorieClient categorieClient, ContextSession session)
    {
        var context = GetContext(session);

        var existingCategorieClient = await context.CategorieClients.FindAsync(categorieClient.Id);
        if (existingCategorieClient != null)
        {
           context.Entry(existingCategorieClient).CurrentValues.SetValues(categorieClient);
            
            await context.SaveChangesAsync();
        }

        return existingCategorieClient;
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        await Delete(id, session);
        return true;
    }
}