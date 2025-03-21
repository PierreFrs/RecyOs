using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public abstract class EuropeRepository : BaseDeletableRepository<ClientEurope, DataContext>,
    IEuropeRepository<ClientEurope>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string column;

    protected EuropeRepository(DataContext context, IHttpContextAccessor httpContextAccessor, string prmColumn) :
        base(context)
    {
        _httpContextAccessor = httpContextAccessor;
        column = prmColumn;
    }

    /// <summary>
    /// Retrieves a <see cref="ClientEurope"/> entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted entities.</param>
    /// <returns>
    /// The <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains the <see cref="ClientEurope"/> entity with the specified ID,
    /// or null if no entity is found.
    /// </returns>
    public async Task<ClientEurope> GetById(int id, ContextSession session, bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(i => i.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all instances of <see cref="ClientEurope"/> from the database.
    /// </summary>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
    /// <returns>An array of <see cref="ClientEurope"/>.</returns>
    public async Task<IList<ClientEurope>> GetAll(ContextSession session, bool includeDeleted = false)
    {
        IQueryable<ClientEurope> query = GetEntities(session, includeDeleted);

        // Apply additional column filter if provided
        query = ApplyColumnFilter(query);

        return await query.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Retrieves a ClientEurope entity by its VAT number.
    /// </summary>
    /// <param name="vat">The VAT number of the client.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">Optional flag to include deleted entities. Default is false.</param>
    /// <returns>The ClientEurope entity with the specified VAT number, or null if not found.</returns>
    public async Task<ClientEurope> GetByVat(string vat, ContextSession session, bool includeDeleted = false)
    {
        var context = GetContext(session); // Directly get the context based on the session

        // Form the base query
        IQueryable<ClientEurope> query = context.ClientEurope.AsQueryable();

        // Apply filter to include or exclude deleted entities
        if (!includeDeleted)
        {
            query = query.Where(entity => !entity.IsDeleted);
        }

        // Retrieve the entity by its Siret
        return await query.AsNoTracking().FirstOrDefaultAsync(entity => entity.Vat == vat);
    }

    /// <summary>
    /// Retrieves a <see cref="ClientEurope"/> entity by codeKerlog.
    /// </summary>
    /// <param name="codeKerlog">The codeKerlog value to search for.</param>
    /// <param name="session">The <see cref="ContextSession"/> instance.</param>
    /// <param name="includeDeleted">A boolean flag indicating whether to include deleted entities.</param>
    /// <returns>
    /// The first <see cref="ClientEurope"/> entity that matches the codeKerlog value,
    /// or null if no entity is found.
    /// </returns>
    public async Task<ClientEurope> GetByCodeKerlog(string codeKerlog, ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(i => i.CodeKerlog == codeKerlog)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a filtered list of ClientEurope entities along with the total count.
    /// </summary>
    /// <param name="filter">The filter to apply to the query.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted entities.</param>
    /// <returns>
    /// A tuple containing the filtered list of ClientEurope entities and the total count.
    /// </returns>
    public async Task<(IEnumerable<ClientEurope>, int)> GetFiltredListWithCount(
        ClientEuropeGridFilter filter,
        ContextSession session,
        bool includeDeleted = false
    )
    {
        var query = ApplyColumnFilter(GetEntities(session, includeDeleted))
            .ApplyFilter(filter);

        // Apply additional column filter if provided
        query = ApplyColumnFilter(query);

        var totalCount = await query.CountAsync();

        var resultList = await query
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToArrayAsync();

        return (resultList, totalCount);
    }

    /// <summary>
    /// Retrieves a <see cref="ClientEurope"/> entity based on the specified <paramref name="codeMkgt"/>.
    /// </summary>
    /// <param name="codeMkgt">The code value of the entity.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">True to include deleted entities, false otherwise.</param>
    /// <returns>The <see cref="ClientEurope"/> entity matching the specified <paramref name="codeMkgt"/>, or null if not found.</returns>
    public async Task<ClientEurope> GetByCodeMkgt(string codeMkgt, ContextSession session, bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(i => i.CodeMkgt == codeMkgt)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a ClientEurope entity by its Odoo ID.
    /// </summary>
    /// <param name="idOdoo">The Odoo ID of the ClientEurope entity.</param>
    /// <param name="session">The ContextSession instance.</param>
    /// <param name="includeDeleted">Whether to include deleted entities. Default is false.</param>
    /// <returns>The ClientEurope entity with the specified Odoo ID, or null if not found.</returns>
    public async Task<ClientEurope> GetByIdOdoo(string idOdoo, ContextSession session, bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(i => i.IdOdoo == idOdoo)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Updates a <see cref="ClientEurope"/> entity.
    /// </summary>
    /// <param name="clientEurope">The <see cref="ClientEurope"/> entity to update.</param>
    /// <param name="session">The <see cref="ContextSession"/> object.</param>
    /// <returns>The updated <see cref="ClientEurope"/> entity.</returns>
    public async Task<ClientEurope> UpdateAsync(ClientEurope clientEurope, ContextSession session)
    {
        var context = GetContext(session);

        var existingClientEurope = await context.ClientEurope      // Sert de comparaison ne pas ecraaser !!!
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == clientEurope.Id);

        if (existingClientEurope == null)
        {
            throw new InvalidOperationException("ClientEurope not found.");
        }
        
        // Update audit fields
        clientEurope.UpdatedBy = GetCurrentUserName();
        clientEurope.UpdatedAt = DateTime.Now;

        // Handle unmodifiable fields
        EnforceUnmodifiableFields(existingClientEurope, clientEurope);

        // Save changes
        try
        {
            context.Entry(clientEurope).State = EntityState.Modified;
            // Exclude properties that should not be modified
            context.Entry(clientEurope).Property(x => x.Vat).IsModified = false;
            context.Entry(clientEurope).Property(x => x.CreatedBy).IsModified = false;
            context.Entry(clientEurope).Property(x => x.CreateDate).IsModified = false;
            
            await context.SaveChangesAsync();

            // Detach the entity if necessary
            context.Entry(clientEurope).State = EntityState.Detached;
        }
        catch (DbUpdateException e)
        {
            throw new DbUpdateException("Erreur lors de la mise à jour de l'établissement client", e);
        }

        return clientEurope;
    }

    private void EnforceUnmodifiableFields(
        ClientEurope originalClientEurope,
        ClientEurope patchedClientEurope)
    {
        // Traitement des champs qui ne sont pas en réecriture libre
        // Appliquez ou ignorez la modification des champs BIC et IBAN en fonction des droits de l'utilisateur
        if (!CanUserUpdateBankInfo())
        {
            patchedClientEurope.Bic = originalClientEurope.Bic;
            patchedClientEurope.Iban = originalClientEurope.Iban;
        }

        // Impossible de modifier l'ID Hubspot une fois qu'il a été défini
        if (originalClientEurope.IdHubspot != null && originalClientEurope.IdHubspot != "")
        {
            patchedClientEurope.IdHubspot = originalClientEurope.IdHubspot;
        }

        // Impossible de modifier l'ID Odoo une fois qu'il a été défini
        if (originalClientEurope.IdOdoo != null && originalClientEurope.IdOdoo != "")
        {
            patchedClientEurope.IdOdoo = originalClientEurope.IdOdoo;
        }

        // Impossible de modifier le code MKGT une fois qu'il a été défini
        if (originalClientEurope.CodeMkgt != null && originalClientEurope.CodeMkgt != "")
        {
            patchedClientEurope.CodeMkgt = originalClientEurope.CodeMkgt;
        }

        // Impossible de modifier le code client gpi une fois qu'il a été défini
        if (originalClientEurope.CodeGpi != null && originalClientEurope.CodeGpi != "")
        {
            patchedClientEurope.CodeGpi = originalClientEurope.CodeGpi;
        }

        // Impossible de modifier le code fournisseur gpi une fois qu'il a été défini
        if (originalClientEurope.FrnCodeGpi != null && originalClientEurope.FrnCodeGpi != "")
        {
            patchedClientEurope.FrnCodeGpi = originalClientEurope.FrnCodeGpi;
        }
        
        // Impossible de modifier le code Shipper Dashdoc une fois qu'il a été défini
        if (originalClientEurope.IdShipperDashdoc != null && originalClientEurope.IdShipperDashdoc != 0)
        {
            patchedClientEurope.IdShipperDashdoc = originalClientEurope.IdShipperDashdoc;
        }
        
        // Impossible de modifier le code Dashdoc une fois qu'il a été défini
        if (originalClientEurope.IdDashdoc != null && originalClientEurope.IdDashdoc != 0)
        {
            patchedClientEurope.IdDashdoc = originalClientEurope.IdDashdoc;
        }
        
        // Inpossible de modifier DateCreMkgt une fois qu'il a été défini
        if (originalClientEurope.DateCreMkgt != null)
        {
            patchedClientEurope.DateCreMkgt = originalClientEurope.DateCreMkgt;
        }
        
        // Inpossible de modifier DateCreGpi une fois qu'il a été défini
        if (originalClientEurope.DateCreGpi != null)
        {
            patchedClientEurope.DateCreGpi = originalClientEurope.DateCreGpi;
        }
        
        // Inpossible de modifier DateCreOdoo une fois qu'il a été défini
        if (originalClientEurope.DateCreOdoo != null)
        {
            patchedClientEurope.DateCreOdoo = originalClientEurope.DateCreOdoo;
        }
        
        // Inpossible de modifier DateCreDashdoc une fois qu'il a été défini
        if (originalClientEurope.DateCreDashdoc != null)
        {
            patchedClientEurope.DateCreDashdoc = originalClientEurope.DateCreDashdoc;
        }
        
        if(patchedClientEurope.CodeMkgt != null && patchedClientEurope.DateCreMkgt == null)
        {
            patchedClientEurope.DateCreMkgt = DateTime.Now;
        }
        
        if(patchedClientEurope.CodeGpi != null && patchedClientEurope.DateCreGpi == null)
        {
            patchedClientEurope.DateCreGpi = DateTime.Now;
        }
        
        if(patchedClientEurope.IdOdoo != null && patchedClientEurope.DateCreOdoo == null)
        {
            patchedClientEurope.DateCreOdoo = DateTime.Now;
        }
        
        if(patchedClientEurope.IdDashdoc != null && patchedClientEurope.DateCreDashdoc == null)
        {
            patchedClientEurope.DateCreDashdoc = DateTime.Now;
        }
    }

    /// <summary>
    /// Check if a particular clientEurope exists in the database based on its id or vat.
    /// </summary>
    /// <param name="clientEurope">The clientEurope to check existence for.</param>
    /// <param name="session">The session used for the database connection.</param>
    /// <returns>True if the clientEurope exists, False otherwise.</returns>
    public async Task<bool> Exists(ClientEurope clientEurope, ContextSession session)
    {
        return await GetEntities(session)
            .Where(x => x.Id == clientEurope.Id || x.Vat == clientEurope.Vat)
            .AsNoTracking()
            .CountAsync() > 0;
    }

    /// <summary>
    /// Creates a new instance of ClientEurope if it doesn't already exist in the database. If it does exist,
    /// updates a specified column's value to true and returns the updated object.
    /// </summary>
    /// <param name="clientEurope">The ClientEurope object to create or update</param>
    /// <param name="session">The context session</param>
    /// <returns>
    /// The created ClientEurope object if it was newly created, or the updated ClientEurope object
    /// if it already existed and its column value was updated.
    /// </returns>
    public async Task<ClientEurope> CreateAsync(ClientEurope clientEurope, ContextSession session)
    {
        var context = GetContext(session);
        var objExists = await GetByVat(clientEurope.Vat, session, true);
        if (objExists != null)
        {
            return await UpdateExistingClientEuropeAsync(clientEurope, session, context);
        }

        var patchedEtab = await SetColumnsFiltersValues(clientEurope, session);
        if (string.IsNullOrEmpty(patchedEtab.CreatedBy))
        {
            patchedEtab.CreatedBy = GetCurrentUserName();
        }

        context.ClientEurope.Add(patchedEtab);

        await context.SaveChangesAsync();

        context.Entry(patchedEtab).State = EntityState.Detached;

        return patchedEtab;
    }

    private async Task<ClientEurope> UpdateExistingClientEuropeAsync(ClientEurope clientEurope, ContextSession session, DataContext context)
    {
        var existingClient = await GetEntities(session, true)
            .FirstOrDefaultAsync(obj => obj.Vat == clientEurope.Vat);

        // Le client existe déjà, mise à jour de la colonne spécifiée à true
        if (!existingClient.IsDeleted && !string.IsNullOrEmpty(column))
        {
            var propertyInfo = existingClient.GetType().GetProperty(column);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(existingClient, true, null);
            }
            else
            {
                throw new ArgumentException(
                    $"La propriété '{column}' n'existe pas sur le type '{existingClient.GetType().Name}' ou n'est pas de type bool.");
            }
        }

        // Reactivate the client if it is soft-deleted
        existingClient.IsDeleted = false;

        // Update properties from etablissementClient
        existingClient.Client = clientEurope.Client;
        existingClient.Fournisseur = clientEurope.Fournisseur;

        context.Entry(existingClient).State = EntityState.Modified;

        await context.SaveChangesAsync();

        context.Entry(existingClient).State = EntityState.Detached;

        return existingClient;
    }
    
    /// <summary>
    /// Applies additional column filter to the query.
    /// </summary>
    /// <param name="query">The original query.</param>
    /// <returns>The modified query with column filter applied.</returns>
    private IQueryable<ClientEurope> ApplyColumnFilter(IQueryable<ClientEurope> query)
    {
        if (!string.IsNullOrEmpty(column) && typeof(ClientEurope).GetProperty(column)?.PropertyType == typeof(bool))
        {
            query = query.Where(x => (bool)EF.Property<object>(x, column));
        }

        return query;
    }

    /// <summary>
    /// Sets the values of column filters for a ClientEurope entity.
    /// </summary>
    /// <param name="clientEurope">The ClientEurope entity for which the column filters values will be set.</param>
    /// <param name="session">The ContextSession object.</param>
    /// <returns>The modified ClientEurope entity with the column filter values set.</returns>
    private async Task<ClientEurope> SetColumnsFiltersValues(ClientEurope clientEurope, ContextSession session)
    {
        IQueryable<ClientEurope> query = GetEntities(session, true);

        // Filter by Id
        query = query.Where(i => i.Vat == clientEurope.Vat);
        var lastEtab = await query.AsNoTracking().FirstOrDefaultAsync();

        if (lastEtab != null)
        {
            clientEurope.Client = lastEtab.Client;
            clientEurope.Fournisseur = lastEtab.Fournisseur;
        }

        return SetCreationFilterValues(clientEurope);
    }

    /// <summary>
    /// Sets the creation filter values for a client in the Europe repository.
    /// </summary>
    /// <param name="clientEurope">The client to set the filter values for.</param>
    /// <returns>The client with the filter values set.</returns>
    private ClientEurope SetCreationFilterValues(ClientEurope clientEurope)
    {
        if (!string.IsNullOrEmpty(column))
        {
            var propertyInfo = clientEurope.GetType().GetProperty(column);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(clientEurope, true, null);
            }
            else
            {
                throw new ArgumentException(
                    $"La propriété '{column}' n'existe pas sur le type '{clientEurope.GetType().Name}' ou n'est pas de type bool.");
            }
        }

        return clientEurope;
    }

    /// <summary>
    /// Gets the current user name.
    /// </summary>
    /// <returns>The current user name.</returns>
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }

    /// <summary>
    /// Determines if the user has permission to update bank information.
    /// </summary>
    /// <returns>
    /// True if the user has the 'write_bank_infos' claim, otherwise false.
    /// </returns>
    public bool CanUserUpdateBankInfo()
    {
        // Vérifier si l'utilisateur a le claim de rôle 'write_bank_infos'
        var hasPermission = _httpContextAccessor.HttpContext.User.HasClaim(c =>
            c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "write_bank_infos");

        return hasPermission;
    }

    public async Task<ClientEurope> DeleteErpCodeAsync(int id, string codeType, ContextSession session)
    {
        var context = GetContext(session);

        var existingEntity = await context.ClientEurope
            .FirstOrDefaultAsync(e => e.Id == id);

        if (existingEntity == null)
        {
            throw new Exception("Entity not found");
        }

        switch (codeType.ToLower())
        {
            case "mkgt":
                existingEntity.CodeMkgt = null;
                existingEntity.DateCreMkgt = null;
                break;
            case "gpi":
                existingEntity.CodeGpi = null;
                existingEntity.DateCreGpi = null;
                break;
            case "frn_gpi":
                existingEntity.FrnCodeGpi = null;
                break;
            case "odoo":
                existingEntity.IdOdoo = null;
                existingEntity.DateCreOdoo = null;
                break;
            default:
                throw new ArgumentException($"Unknown code type: {codeType}");
        }


        await context.SaveChangesAsync();

        return existingEntity;
    }

}