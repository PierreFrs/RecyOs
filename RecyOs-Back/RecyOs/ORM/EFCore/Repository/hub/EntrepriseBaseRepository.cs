using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class EntrepriseBaseRepository: BaseDeletableRepository<EntrepriseBase, DataContext>, 
    IEntrepriseBaseRepository<EntrepriseBase>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EntrepriseBaseRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(IEnumerable<EntrepriseBase>, int)> GetFiltredListWithCount(EntrepriseBaseGridFilter filter, 
        ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted).ApplyFilter(filter);
        return (
            await query
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToArrayAsync(),
            await query.CountAsync());
    }

    public async Task<EntrepriseBase> GetBySiren(string siren, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Siren == siren)
            .Include(x => x.EntrepriseNDCover)
            .FirstOrDefaultAsync();
    }

    public async Task<EntrepriseBase> Update(EntrepriseBase entrepriseBase, ContextSession session)
    {
        var objectExists = await Exists(entrepriseBase, session);
        
        if(entrepriseBase.Id == 0)
        {
            // Siren is unique, so we can use it to find the object because Entity Framework can't update an
            // object with an Id of 0. This is a workaround.
            // get the id of the object
            var entrepriseBaseId = await GetEntities(session)
                .AsNoTracking()  // Pas besoin de tracker les modifications car on ne fait que lire
                .Where(x => x.Siren == entrepriseBase.Siren)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        
            entrepriseBase.Id = entrepriseBaseId;
        }

        var context = GetContext(session);
        entrepriseBase.UpdatedBy = GetCurrentUserName();
        entrepriseBase.UpdatedAt= DateTime.Now;
        foreach (var entity in context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        
        context.Entry(entrepriseBase).State = objectExists ? EntityState.Modified : EntityState.Added;
        
        context.Entry(entrepriseBase).Property(x => x.Siren).IsModified = false;
        
        context.Entry(entrepriseBase).Property(x => x.CreateDate).IsModified = false;
        context.Entry(entrepriseBase).Property(x => x.CreatedBy).IsModified = false;

        try
        {
            await context.SaveChangesAsync();
            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;   // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return entrepriseBase;
    }

    public override async Task<bool> Exists(EntrepriseBase obj, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .AsNoTracking()    // Pas besoin de tracker les modifications car on ne fait que lire
            .Where(x => x.Siren == obj.Siren)
            .CountAsync() > 0;
    }
    
    public async Task<bool> Exists(EntrepriseBase obj, ContextSession session)
    {
        return await GetEntities(session, true)
            .AsNoTracking()   // Pas besoin de tracker les modifications car on ne fait que lire
            .Where(x => x.Siren == obj.Siren)
            .CountAsync() > 0;
    }

    public async Task<EntrepriseBase> Create(EntrepriseBase entrepriseBase, ContextSession session)
    {
        var context = GetContext(session);
        var objectExists = await Exists(entrepriseBase, session);
        if (objectExists)
        {
            return await Update(entrepriseBase, session);
        }
        context.EntrepriseBase.Add(entrepriseBase);
        try
        {
            await context.SaveChangesAsync();
            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return entrepriseBase;
    }
    
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }
}