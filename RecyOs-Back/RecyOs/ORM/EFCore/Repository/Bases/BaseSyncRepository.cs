using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.EFCore.Repository;

/// <summary>
/// Base class for synchronous repositories that handle synchronization of entities.
/// </summary>
/// <typeparam name="TType">The type of the entity.</typeparam>
/// <typeparam name="TContext">The type of the data context.</typeparam>
public abstract class BaseSyncRepository<TType, TContext>
    where TType : TrackedEntity, new()
    where TContext : DataContext
{
    private readonly TContext _dbContext;
    private readonly String _moduleName;

    /// <summary>
    /// Initializes a new instance of the BaseSyncRepository class with the specified context and module name.
    /// </summary>
    /// <param name="context">The database context object.</param>
    /// <param name="moduleName">The name of the module.</param>
    protected BaseSyncRepository(TContext context, String moduleName)
    {
        _dbContext = context;
        _moduleName = moduleName;
    }

    /// <summary>
    /// Retrieves the entities that were created between the last retrieval and the current time. </summary> <param name="session">The context session in which to retrieve the entities.</param> <returns>
    /// An IQueryable collection of entities created between the last retrieval and the current time. </returns>
    /// /
    protected IQueryable<TType> GetCreatedEntities(ContextSession session)
    {
        DateTime? lastCreate = getLastCreDateTime();
        DateTime now = DateTime.Now;
        
        var context = GetContext(session);
        var returnValue = context.Set<TType>().Where(ec => ec.CreateDate > lastCreate
                                                           && ec.CreateDate <= now).AsQueryable().AsNoTracking();
        setLastCreDateTime(now);
        return returnValue;
    }

    /// <summary>
    /// Retrieves the updated entities of type TType from the specified session context.
    /// </summary>
    /// <param name="session">The context session to retrieve the entities from.</param>
    /// <returns>An <see cref="IQueryable{TType}"/> containing the updated entities.</returns>
    protected IQueryable<TType> GetUpdatedEntities(ContextSession session)
    {
        DateTime? lastUpdate = getLastUpdDateTime();
        DateTime now = DateTime.Now;
        
        var context = GetContext(session);
        var returnValue = context.Set<TType>().Where(ec => ec.UpdatedAt> lastUpdate
                                                           && ec.UpdatedAt <= now).AsQueryable().AsNoTracking();
        setLastUpdDateTime(now);
        return returnValue;
    }

    /// <summary>
    /// Gets the data context for the specified session.
    /// </summary>
    /// <param name="session">The session to associate with the data context.</param>
    /// <returns>The data context associated with the specified session.</returns>
    protected DataContext GetContext(ContextSession session)
    {
        _dbContext.Session = session;
        return _dbContext;
    }

    /// <summary>
    /// Gets the last update date and time for the current module from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time as <see cref="DateTime"/>?, or null if there are no records found.</returns>
    protected DateTime? getLastUpdDateTime()
    {
        return _dbContext.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals(_moduleName))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last creation date and time from the engine sync status table.
    /// </summary>
    /// <returns>
    /// The last creation date and time, or null if no records were found.
    /// </returns>
    protected DateTime? getLastCreDateTime()
    {
        return _dbContext.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals(_moduleName))
            .Select(ess => ess.LastCreate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Sets the last update date and time for the specified module in the database.
    /// </summary>
    /// <param name="prmValue">The new value for the last update date and time.</param>
    protected void setLastUpdDateTime(DateTime prmValue)
    {
        _dbContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals(_moduleName))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// Sets the last creation date and time for the specified module.
    /// </summary>
    /// <param name="prmDateTime">The date and time to set as the last creation date and time.</param>
    protected void setLastCreDateTime(DateTime prmDateTime)
    {
        _dbContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals(_moduleName))
            .FirstOrDefault()
            .LastCreate = prmDateTime;
        _dbContext.SaveChanges();
    }
}