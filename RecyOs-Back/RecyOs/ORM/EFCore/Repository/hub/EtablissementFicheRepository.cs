using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class EtablissementFicheRepository: BaseDeletableRepository<EtablissementFiche, DataContext>, IEtablissementFicheRepository<EtablissementFiche>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EtablissementFicheRepository(
        DataContext context, 
        IHttpContextAccessor httpContextAccessor
        ) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(IEnumerable<EtablissementFiche>, int)> GetFiltredListWithCount(EtablissementFicheGridFilter filter, ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted).ApplyFilter(filter);
        return (
            await query
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToArrayAsync(),
            await query.CountAsync());
    }

    public async Task<EtablissementFiche> GetBySiret(string siret, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Siret == siret)
            .FirstOrDefaultAsync();
    }

    public async Task<EtablissementFiche> UpdateAsync(EtablissementFiche etablissementFiche, ContextSession session)
    {
        var objectExists = await Exists(etablissementFiche, session);
        if(etablissementFiche.Id == 0)
        {
            // Siret is unique, so we can use it to find the object because Entity Framework can't update an
            // object with an Id of 0. This is a workaround.
            // get the id of the object
            var etablissementFicheId = await GetEntities(session)
                .AsNoTracking()  // Pas besoin de tracker les modifications car on ne fait que lire
                .Where(x => x.Siret == etablissementFiche.Siret)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        
            etablissementFiche.Id = etablissementFicheId;
        }
        var context = GetContext(session);
        etablissementFiche.UpdatedBy = GetCurrentUserName();
        etablissementFiche.UpdatedAt= DateTime.Now;
        context.Entry(etablissementFiche).State = objectExists ? EntityState.Modified : EntityState.Added;
        context.Entry(etablissementFiche).Property(x => x.Siret).IsModified = false;   // Siret is unique, so we can't update it
        context.Entry(etablissementFiche).Property(x => x.CreateDate).IsModified = false;  // CreateDate is set by the database
        context.Entry(etablissementFiche).Property(x => x.CreatedBy).IsModified = false; // CreatedBy is set by the database

        await context.SaveChangesAsync();
        foreach (var entity in context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        
        return etablissementFiche;
    }
    
    public override async Task<bool> Exists(EtablissementFiche obj, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .AsNoTracking()  // Pas besoin de tracker les modifications car on ne fait que lire
            .CountAsync(x => x.Siret == obj.Siret) > 0;
    }
    
    public async Task<EtablissementFiche> Create(EtablissementFiche etablissementFiche, ContextSession session)
    {
        var context = GetContext(session);
        var objectExists = await Exists(etablissementFiche, session, true);
        if (objectExists)
        {
            var existingEntity = await GetEntities(session, true)
                .AsNoTracking() // Pas besoin de tracker les modifications car on ne fait que lire
                .FirstOrDefaultAsync(x => x.Siret == etablissementFiche.Siret);

            if (existingEntity.IsDeleted)
            {
                existingEntity.IsDeleted = false;
                existingEntity.UpdatedBy = GetCurrentUserName();
                existingEntity.UpdatedAt = DateTime.Now;
                context.Entry(existingEntity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                context.Entry(existingEntity).State = EntityState.Detached;
                return existingEntity;
            }
            else if (existingEntity.IsDeleted == false)
            {
                throw new Exception("The object EtablissementFiche with the siret " + etablissementFiche.Siret + " already exists.");
            }
        }
        context.EtablissementFiche.Add(etablissementFiche);
        await context.SaveChangesAsync();
        foreach (var entity in context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        return etablissementFiche;
    }
    
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }
}