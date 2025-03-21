using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.gpiSync;

namespace RecyOs.ORM.EFCore.Repository.gpiSync;

/// <summary>
/// Repository class for synchronizing Gpi Etablissement clients.
/// </summary>
public class GpiSyncEtablissementClientRepository : BaseSyncRepository<EtablissementClient, DataContext>, IGpiSyncEtablissementClientRepository<EtablissementClient>
{
    public GpiSyncEtablissementClientRepository(DataContext context): base(context, "GpiClientModule")
    {
       
    }

    /// <summary>
    /// Retrieves a list of created EtablissementClient objects based on the provided session context.
    /// </summary>
    /// <param name="session">The session context.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a list of EtablissementClient objects.</returns>
    public async Task<IList<EtablissementClient>> GetCreatedEtablissementClient(ContextSession session)
    {
        DateTime? lastCreate = getLastCreDateTime();
        DateTime now = DateTime.Now;
        
        var context = GetContext(session);
        var returnValue = await context.Set<EtablissementClient>()
            .Where(ec => ec.DateCreGpi > lastCreate && ec.DateCreGpi <= now
                                                    && ((ec.CodeGpi != null && ec.CodeGpi != "")
                                                    || (ec.FrnCodeGpi != null && ec.FrnCodeGpi != "")))
            .Include(e => e.EntrepriseBase).AsNoTracking().ToListAsync();
        setLastCreDateTime(now);
        return returnValue;
    }

    /// <summary>
    /// Retrieves the list of updated EtablissementClient entities in the given session.
    /// </summary>
    /// <param name="session">The session object containing the context for retrieving the entities.</param>
    /// <returns>A task representing the asynchronous operation that returns a list of updated EtablissementClient entities.</returns>
    public async Task<IList<EtablissementClient>> GetUpdatedEtablissementClient(ContextSession session)
    {
        return await this.GetUpdatedEntities(session)
            .Where(e => ((e.CodeGpi != null && e.CodeGpi != "") 
                         || (e.FrnCodeGpi != null && e.FrnCodeGpi != "")))
            .Include(e => e.EntrepriseBase).ToListAsync();
    }
}