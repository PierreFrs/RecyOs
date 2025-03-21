using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.gpiSync;

namespace RecyOs.ORM.EFCore.Repository.gpiSync;

/// <summary>
/// Represents a repository for managing GpiSyncClientEurope entities.
/// </summary>
public class GpiSyncClientEuropeRepository : BaseSyncRepository<ClientEurope, DataContext>, IGpiSyncClientEuropeRepository<ClientEurope>
{
    public GpiSyncClientEuropeRepository(DataContext context) : base(context, "GpiEuropeClientModule")
    {
    }

    /// <summary>
    /// Retrieves a list of created clients in Europe within a specified time period.
    /// </summary>
    /// <param name="session">The session context.</param>
    /// <returns>A list of <see cref="ClientEurope"/> objects representing the created clients.</returns>
    public async Task<IList<ClientEurope>> GetCreatedClientEurope(ContextSession session)
    {
        DateTime? lastCreate = getLastCreDateTime();
        DateTime now = DateTime.Now;

        var context = GetContext(session);
        var returnValue = await context.Set<ClientEurope>()
            .Where(ec => ec.DateCreGpi > lastCreate && ec.DateCreGpi <= now
                                                    && ((ec.CodeGpi != null && ec.CodeGpi != "") 
                                                    || (ec.FrnCodeGpi != null && ec.FrnCodeGpi != "")))
                                                    .AsNoTracking().ToListAsync();
        setLastCreDateTime(now);
        return returnValue;
    }

    /// Retrieves the updated ClientEurope entities from the specified session.
    /// @param session The ContextSession to use for retrieving the entities.
    /// @return A Task that represents the asynchronous operation. The task result contains a list of updated ClientEurope entities.
    /// /
    public async Task<IList<ClientEurope>> GetUpdatedClientEurope(ContextSession session)
    {
        return await this.GetUpdatedEntities(session)
            .Where(e => ((e.CodeGpi != null && e.CodeGpi != "") || (e.FrnCodeGpi != null && e.FrnCodeGpi != "")))
            .ToListAsync();
    }
}