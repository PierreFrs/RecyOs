using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.OdooImporter.Interfaces;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Repositories;

public class SocieteRepository : ISocieteRepository
{
    private readonly RecyOs.Helpers.DataContext _dbContext;

    public SocieteRepository(IOdooImporterDbContext odooImporterDbContext)
    {
        _dbContext = odooImporterDbContext.GetContext();
    }

    public async Task<Societe?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var query = _dbContext.Set<Societe>().AsQueryable();
        
        if (!includeDeleted)
            query = query.Where(s => !s.IsDeleted);

        return await query.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Societe?> GetByIdOdooAsync(string idOdoo, bool includeDeleted = false)
    {
        var query = _dbContext.Set<Societe>().AsQueryable();
        
        if (!includeDeleted)
            query = query.Where(s => !s.IsDeleted);

        return await query.FirstOrDefaultAsync(s => s.IdOdoo == idOdoo);
    }

    public async Task<IEnumerable<Societe>> GetAllAsync(bool includeDeleted = false)
    {
        var query = _dbContext.Set<Societe>().AsQueryable();
        
        if (!includeDeleted)
            query = query.Where(s => !s.IsDeleted);

        return await query.ToListAsync();
    }

    public async Task<Societe> CreateAsync(Societe societe)
    {
        await _dbContext.Set<Societe>().AddAsync(societe);
        await _dbContext.SaveChangesAsync();
        return societe;
    }

    public async Task<Societe?> UpdateAsync(Societe societe)
    {
        var existingSociete = await GetByIdAsync(societe.Id);
        if (existingSociete == null)
            return null;

        _dbContext.Entry(existingSociete).CurrentValues.SetValues(societe);
        await _dbContext.SaveChangesAsync();
        return existingSociete;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var societe = await GetByIdAsync(id);
        if (societe == null)
            return false;

        societe.IsDeleted = true;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id, bool includeDeleted = false)
    {
        var query = _dbContext.Set<Societe>().AsQueryable();
        
        if (!includeDeleted)
            query = query.Where(s => !s.IsDeleted);

        return await query.AnyAsync(s => s.Id == id);
    }

    public async Task<bool> ExistsByIdOdooAsync(string idOdoo, bool includeDeleted = false)
    {
        var query = _dbContext.Set<Societe>().AsQueryable();
        
        if (!includeDeleted)
            query = query.Where(s => !s.IsDeleted);

        return await query.AnyAsync(s => s.IdOdoo == idOdoo);
    }
}
