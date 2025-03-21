// /** BaseRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 07/01/2023
//  * Code développé pour le projet : Archimede.Common
//  * Adapté pour le projet : RecyOs Server
//  */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.EFCore.Repository;

public abstract class BaseRepository<TType, TContext>
    where TType : BaseEntity, new()
    where TContext : DataContext
{
    private readonly TContext _dbContext;

    protected BaseRepository(TContext context)
    {
        _dbContext = context;
    }

    protected IQueryable<TType> GetEntities(ContextSession session)
    {
        var context = GetContext(session);
        return context.Set<TType>().AsQueryable().AsNoTracking();
    }

    protected TContext GetContext(ContextSession session)
    {
        _dbContext.Session = session;
        return _dbContext;
    }

    public virtual async Task<IEnumerable<TType>> GetList(ContextSession session)
    {
        return await GetEntities(session).AsNoTracking().ToListAsync();
    }

    public virtual async Task<TType> Get(int id, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public virtual async Task<bool> Exists(TType obj, ContextSession session)
    {
        return await GetEntities(session)
            .Where(x => x.Id == obj.Id)
            .CountAsync() > 0;
    }

    public virtual async Task<TType> Edit(TType obj, ContextSession session)
    {
        var objectExists = await Exists(obj, session);
        var context = GetContext(session);
        var trackedEntity = context.Set<TType>().Local.FirstOrDefault(e => e.Id == obj.Id);
        if (trackedEntity != null)
        {
            context.Entry(trackedEntity).State = EntityState.Detached;
        }
        context.Entry(obj).State = objectExists ? EntityState.Modified : EntityState.Added;
        await context.SaveChangesAsync();
        return obj;
    }

    public virtual async Task Delete(int id, ContextSession session)
    {
        var context = GetContext(session);
        var itemToDelete = new TType {Id = id};
        context.Entry(itemToDelete).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }
}