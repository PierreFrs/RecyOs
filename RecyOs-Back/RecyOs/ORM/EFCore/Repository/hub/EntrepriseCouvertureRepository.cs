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

public class EntrepriseCouvertureRepository : 
    BaseRepository<EntrepriseCouverture, DataContext>,
    IEntrepriseCouvertureRepository<EntrepriseCouverture>
{
    public EntrepriseCouvertureRepository(DataContext context) : base(context)
    {
    }

    public override async Task<EntrepriseCouverture> Get(int id, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .Include(x => x.EntrepriseBase)
            .FirstOrDefaultAsync();
    }
    
    public async Task<(IEnumerable<EntrepriseCouverture>, int)> GetFilteredListWithCount(EntrepriseCouvertureGridFilter filter, 
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
    
    public async Task<EntrepriseCouverture> GetBySiren(string siren, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Siren == siren)
            .FirstOrDefaultAsync();
    }

    public async Task<EntrepriseCouverture> Update(EntrepriseCouverture entrepriseCouverture, ContextSession session)
    {
        var context = GetContext(session);
        var objectExists = await Exists(entrepriseCouverture, session);
        
        if (objectExists)
        {
            var existingEntity = await context.EntrepriseCouverture
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Siren == entrepriseCouverture.Siren);

            if(existingEntity != null)
            {
                entrepriseCouverture.Id = existingEntity.Id;
            }
        }else
        {
            if(entrepriseCouverture.Id != 0) return null;
        }
        context.Entry(entrepriseCouverture).State = objectExists ? EntityState.Modified : EntityState.Added;

        if (string.IsNullOrEmpty(entrepriseCouverture.Siren))
        {
            context.Entry(entrepriseCouverture).Property(x => x.Siren).IsModified = false;
        }
        
        await context.SaveChangesAsync();
        return entrepriseCouverture;
    }
    
    public override async Task<bool> Exists(EntrepriseCouverture entrepriseCouverture, ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()  // Pas besoin de tracker les modifications car on ne fait que lire
            .Where(obj => obj.Siren == entrepriseCouverture.Siren)
            .AnyAsync();
    }
}