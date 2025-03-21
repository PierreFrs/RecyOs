using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using NLog;

namespace RecyOs.ORM.EFCore.Repository;

public class SocieteBaseRepository : BaseDeletableRepository<Societe, DataContext>, ISocieteBaseRepository
{
    public SocieteBaseRepository(DataContext context) : base(context)
    {
    }
    
    public async Task<Societe> CreateAsync(Societe societe, ContextSession session)
    {
        var context = GetContext(session);
        context.Add(societe);
        await context.SaveChangesAsync();
        return societe;
    }

    public async Task<IReadOnlyList<Societe>> GetListAsync(bool includeDeleted = false)
    {
        return await GetEntities(new ContextSession(), includeDeleted).AsNoTracking().ToListAsync();
    }

    public async Task<Societe> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Societe> UpdateAsync(Societe societe, ContextSession session)
    {
        var context = GetContext(session);
        var societeToUpdate = await context.Societes.FindAsync(societe.Id);
        if (societeToUpdate == null) return null;

        context.Entry(societeToUpdate).CurrentValues.SetValues(societe);
        await context.SaveChangesAsync();
        return societeToUpdate;
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        await Delete(id, session);
                return true;
    }
    
    public async Task<bool> Exists(Societe societe, ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()
            .Where(x => x.Id == societe.Id)
            .CountAsync() > 0;
    }

    public async Task<(IEnumerable<Societe>,int)> GetDataForGrid(SocieteGridFilter filter, ContextSession session, bool includeDeleted = false)
    {
            var query = GetEntities(session, includeDeleted).ApplyFilter(filter);

            var count = await query.CountAsync();
            var societes = await query
                .Skip(filter.PageSize * filter.PageNumber)
                .Take(filter.PageSize)
                .ToArrayAsync();

            return (societes, count);


    }
}