using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class EntrepriseNDCoverRepository : 
    BaseRepository<EntrepriseNDCover, DataContext>, 
    IEntrepriseNDCoverRepository<EntrepriseNDCover>
{
    public EntrepriseNDCoverRepository(DataContext context) : base(context)
    {
    }

    public override async Task<EntrepriseNDCover> Get(int id, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .Include(x => x.EntrepriseBase)
            .FirstOrDefaultAsync();
    }
    
    public async Task<(IEnumerable<EntrepriseNDCover>, int)> GetFilteredListWithCount(EntrepriseNDCoverGridFilter filter, 
        ContextSession session)
    {
        var query = GetEntities(session)
            .Include(x => x.EntrepriseBase)
            .ApplyFilter(filter);
        return (
            await query
                .Skip(filter.PageSize * (filter.PageNumber))
                .Take(filter.PageSize)
                .ToArrayAsync(),
            await query.CountAsync());
    }
    
    public async Task<EntrepriseNDCover> GetBySiren(string siren, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Siren == siren)
            .FirstOrDefaultAsync();
    }

    public async Task<EntrepriseNDCover> Update(EntrepriseNDCover entrepriseNDCover, ContextSession session)
    {
        var context = GetContext(session);
        
        var objectExists = await Exists(entrepriseNDCover, session);

        if (objectExists)
        {
            var existingEntity = await context.EntrepriseNDCovers
                .FirstOrDefaultAsync(e => e.Siren == entrepriseNDCover.Siren);

            if(existingEntity != null)
            {
                entrepriseNDCover.Id = existingEntity.Id;
            }
        }
        
        var trackedEntity = context.EntrepriseNDCovers.Local
            .FirstOrDefault(e => e.Id == entrepriseNDCover.Id);
        
        if (trackedEntity != null)
        {
            context.Entry(trackedEntity).CurrentValues.SetValues(entrepriseNDCover);
        }
        else
        {
            context.EntrepriseNDCovers.Update(entrepriseNDCover);
        }
        
        await context.SaveChangesAsync();
        return entrepriseNDCover;
    }

    public override async Task<bool> Exists(EntrepriseNDCover entrepriseNDCover, ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()  // Pas besoin de tracker les modifications car on ne fait que lire
            .Where(obj => obj.Siren == entrepriseNDCover.Siren)
            .AnyAsync();
    }
}