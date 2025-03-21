using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository.hub;

public abstract class EtablissementRepository : BaseDeletableRepository<EtablissementClient, DataContext>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEntrepriseBaseRepository<EntrepriseBase> _entrepriseBaseRepository;
    private readonly string column;
    protected EtablissementRepository(DataContext context,
        IHttpContextAccessor httpContextAccessor,
        IEntrepriseBaseRepository<EntrepriseBase> entrepriseBaseRepository,
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository,
        string prmColumn) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
        _entrepriseBaseRepository = entrepriseBaseRepository;
        column = prmColumn;
    }
    
    /// <summary>
    /// Retrieves an instance of EtablissementClient by its Id and a specified column value.
    /// </summary>
    /// <param name="id">The identifier of the EtablissementClient to retrieve.</param>
    /// <param name="session">The ContextSession object representing the active session.</param>
    /// <param name="includeDeleted">Specify whether to include deleted entities in the result (default: false).</param>
    /// <param name="columnName">The name of the column to filter.</param>
    /// <returns>The EtablissementClient instance with the specified Id and matching column value, or null if not found.</returns>
    public async Task<EtablissementClient> GetById(
        int id,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(i => i.Id == id)
            .Include(x => x.EntrepriseBase)
            .ThenInclude(x => x.EntrepriseNDCover)
            .Include(x => x.EtablissementFiche)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all instances of <see cref="EtablissementClient"/> from the database.
    /// </summary>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
    /// <returns>An array of <see cref="EtablissementClient"/>.</returns>
    public async Task<IEnumerable<EtablissementClient>> GetAll(ContextSession session, bool includeDeleted = false)
    {
        IQueryable<EtablissementClient> query = GetEntities(session, includeDeleted);

        // Apply additional column filter if provided
        query = ApplyColumnFilter(query);

        return await query.AsNoTracking().ToListAsync();
    }
    
    /// <summary>
    /// Récupère une liste filtrée d'établissements clients et le compte total correspondant à ce filtre.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la liste d'établissements clients.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus.</param>
    /// <param name="columnName">Le nom de la colonne à filtrer (doit être de type booléen).</param>
    /// <returns>Un tuple contenant une liste d'établissements clients et le compte total correspondant au filtre.</returns>
    public async Task<(IEnumerable<EtablissementClient>, int)> GetFiltredListWithCount(
        EtablissementClientGridFilter filter,
        ContextSession session,
        bool includeDeleted = false)
    {
        var query = ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Include(x => x.EntrepriseBase)
            .ThenInclude(x => x.EntrepriseNDCover)
            .ApplyFilter(filter);

        var totalCount = await query.CountAsync();

        var resultList = await query
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize)
            .ToArrayAsync();

        return (resultList, totalCount);
    }
    
    /// <summary>
    /// Récupère un établissement client en utilisant son SIRET et en option un filtre basé sur une colonne booléenne.
    /// </summary>
    /// <param name="siret">Le SIRET de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche.</param>
    /// <param name="columnName">Le nom de la colonne à filtrer (doit être de type booléen).</param>
    /// <returns>L'établissement client correspondant au SIRET fourni, ou null s'il n'est pas trouvé.</returns>
    public async Task<EtablissementClient> GetBySiret(
        string siret,
        ContextSession session,
        bool includeDeleted = false)
    {
        var context = GetContext(session); // Directly get the context based on the session

        // Form the base query
        IQueryable<EtablissementClient> query = context.EtablissementClient.AsQueryable();

        // Apply filter to include or exclude deleted entities
        if (!includeDeleted)
        {
            query = query.Where(entity => !entity.IsDeleted);
        }

        // Retrieve the entity by its Siret
        return await query.AsNoTracking().FirstOrDefaultAsync(entity => entity.Siret == siret);
    }


    /// <summary>
    /// Retrieves an instance of <see cref="EtablissementClient"/> by its <paramref name="codeKerlog"/>.
    /// </summary>
    /// <param name="codeKerlog">The codeKerlog of the <see cref="EtablissementClient"/>.</param>
    /// <param name="session">The session context.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities in the result.</param>
    /// <param name="columnName">The name of the column to apply an additional filter based on its value. Must be of type bool.</param>
    /// <returns>An instance of <see cref="EtablissementClient"/> if found, otherwise null.</returns>
    public async Task<EtablissementClient> GetByCodeKerlog(
        string codeKerlog,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(obj => obj.CodeKerlog == codeKerlog)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    /// <summary>
    /// Récupère un établissement client basé sur son code Mkgt.
    /// </summary>
    /// <param name="codeMkgt">Le code Mkgt de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche. Par défaut à false.</param>
    /// <param name="columnName">Le nom de la colonne booléenne à filtrer, où la valeur doit être true.</param>
    /// <returns>L'établissement client correspondant au code Mkgt fourni et à la condition de la colonne spécifiée, ou null si aucun établissement n'a été trouvé.</returns>
    public async Task<EtablissementClient> GetByCodeMkgt(
        string codeMkgt,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(obj => obj.CodeMkgt == codeMkgt)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    /// <summary>
    /// Récupère un établissement client basé sur son identifiant Odoo.
    /// </summary>
    /// <param name="idOdoo">L'identifiant Odoo de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche. Par défaut à false.</param>
    /// <param name="columnName">Le nom de la colonne booléenne à filtrer, où la valeur doit être true.</param>
    /// <returns>L'établissement client correspondant à l'identifiant Odoo fourni et à la condition de la colonne spécifiée, ou null si aucun établissement n'a été trouvé.</returns>
    public async Task<EtablissementClient> GetByIdOdoo(
        string idOdoo,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(obj => obj.IdOdoo == idOdoo)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves an <see cref="EtablissementClient"/> by its GPI code.
    /// </summary>
    /// <param name="codeGpi">The GPI code of the <see cref="EtablissementClient"/>.</param>
    /// <param name="session">The <see cref="ContextSession"/>.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted <see cref="EtablissementClient"/>.</param>
    /// <param name="columnName">The name of a column to apply an additional filter if provided.</param>
    /// <returns>The <see cref="EtablissementClient"/> with the specified GPI code.</returns>
    public async Task<EtablissementClient> GetByCodeGpi(
        string codeGpi,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await ApplyColumnFilter(GetEntities(session, includeDeleted))
            .Where(obj => obj.CodeGpi == codeGpi)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all EtablissementClient sharing the same siren from the database.
    /// </summary>
    /// <param name="siren">The siren of the EtablissementClient to retrieve.</param>
    /// <returns>A List of EtablissementClient sharing the same siren.</returns>
    public async Task<IEnumerable<EtablissementClient>> GetEtablissementGroupBySirenAsync(
        string siren,
        ContextSession session,
        bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(x => x.Siren == siren)
            .Include(x => x.EtablissementFiche)
            .ToListAsync();
    }

    /// <summary>
    /// Met à jour un établissement client dans la base de données.
    /// </summary>
    /// <param name="etablissementClient">L'objet EtablissementClient à mettre à jour.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>L'établissement client mis à jour.</returns>
    /// <remarks>
    /// Cette méthode vérifie également si certaines propriétés, comme le code MKGT et l'ID Odoo, sont renseignées.
    /// Si c'est le cas, elle met à jour la date de création associée. Si une propriété est nulle ou non renseignée,
    /// elle s'assure que la propriété n'est pas modifiée lors de la mise à jour dans la base de données.
    /// Une exception de type DbUpdateException peut être déclenchée en cas d'erreur lors de la mise à jour.
    /// </remarks>
    public async Task<EtablissementClient> UpdateAsync(EtablissementClient etablissementClient, ContextSession session)
    {
        var context = GetContext(session);

        // Load the existing entity from the database
        var existingEntity = await context.EtablissementClient.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == etablissementClient.Id);

        if (existingEntity == null)
        {
            throw new InvalidOperationException("EtablissementClient not found.");
        }
        
        // Update audit fields
        etablissementClient.UpdatedBy = GetCurrentUserName();
        etablissementClient.UpdatedAt = DateTime.Now;
        
        if(etablissementClient.CodeMkgt != null && etablissementClient.DateCreMKGT == null)
        {
            etablissementClient.DateCreMKGT = DateTime.Now;
        }
        
        if(etablissementClient.CodeGpi != null && etablissementClient.DateCreGpi == null)
        {
            etablissementClient.DateCreGpi = DateTime.Now;
        }
        
        if(etablissementClient.IdOdoo != null && etablissementClient.DateCreOdoo == null)
        {
            etablissementClient.DateCreOdoo = DateTime.Now;
        }
        
        if(etablissementClient.IdDashdoc != null && etablissementClient.DateCreDashdoc == null)
        {
            etablissementClient.DateCreDashdoc = DateTime.Now;
        }

        // Handle unmodifiable fields
        EnforceUnmodifiableFields(existingEntity, etablissementClient);

        // Save changes
        try
        {
            context.Entry(etablissementClient).State = EntityState.Modified;
            // Exclude properties that should not be modified
            context.Entry(etablissementClient).Property(x => x.Siret).IsModified = false;
            context.Entry(etablissementClient).Property(x => x.CreatedBy).IsModified = false;
            context.Entry(etablissementClient).Property(x => x.CreateDate).IsModified = false;
            
            await context.SaveChangesAsync();

            // Detach the entity if necessary
            context.Entry(etablissementClient).State = EntityState.Detached;
        }
        catch (DbUpdateException e)
        {
            throw new DbUpdateException("Erreur lors de la mise à jour de l'établissement client", e);
        }

        return etablissementClient;
    }

    private void EnforceUnmodifiableFields(EtablissementClient originalEtablissementClient,
        EtablissementClient patchedEtablissementClient)
    {
        // Traitement des champs qui ne sont pas en réecriture libre
        // Appliquez ou ignorez la modification des champs BIC et IBAN en fonction des droits de l'utilisateur
        if (!CanUserUpdateBankInfo())
        {
            patchedEtablissementClient.Bic = originalEtablissementClient.Bic;
            patchedEtablissementClient.Iban = originalEtablissementClient.Iban;
        }

        // Impossible de modifier l'ID Hubspot une fois qu'il a été défini
        if (originalEtablissementClient.IdHubspot != null && originalEtablissementClient.IdHubspot != "")
        {
            patchedEtablissementClient.IdHubspot = originalEtablissementClient.IdHubspot;
        }

        // Impossible de modifier l'ID Odoo une fois qu'il a été défini
        if (originalEtablissementClient.IdOdoo != null && originalEtablissementClient.IdOdoo != "")
        {
            patchedEtablissementClient.IdOdoo = originalEtablissementClient.IdOdoo;
        }

        // Impossible de modifier le code MKGT une fois qu'il a été défini
        if (originalEtablissementClient.CodeMkgt != null && originalEtablissementClient.CodeMkgt != "")
        {
            patchedEtablissementClient.CodeMkgt = originalEtablissementClient.CodeMkgt;
        }

        // Impossible de modifier le code client gpi une fois qu'il a été défini
        if (originalEtablissementClient.CodeGpi != null && originalEtablissementClient.CodeGpi != "")
        {
            patchedEtablissementClient.CodeGpi = originalEtablissementClient.CodeGpi;
        }

        // Impossible de modifier le code fournisseur gpi une fois qu'il a été défini
        if (originalEtablissementClient.FrnCodeGpi != null && originalEtablissementClient.FrnCodeGpi != "")
        {
            patchedEtablissementClient.FrnCodeGpi = originalEtablissementClient.FrnCodeGpi;
        }
        
        // Impossible de modifier le code Dashdoc une fois qu'il a été défini
        if (originalEtablissementClient.IdDashdoc != null && originalEtablissementClient.IdDashdoc != 0)
        {
            patchedEtablissementClient.IdDashdoc = originalEtablissementClient.IdDashdoc;
        }
        
        // Impossible de modifier le code Shipper Dashdoc une fois qu'il a été défini
        if (originalEtablissementClient.IdShipperDashdoc != null && originalEtablissementClient.IdShipperDashdoc != 0)
        {
            patchedEtablissementClient.IdShipperDashdoc = originalEtablissementClient.IdShipperDashdoc;
        }
        
        // Impossible de modifier DateCreMKGT une fois qu'il a été défini
        if (originalEtablissementClient.DateCreMKGT != null)
        {
            patchedEtablissementClient.DateCreMKGT = originalEtablissementClient.DateCreMKGT;
        }
        
        // Impossible de modifier DateCreGpi une fois qu'il a été défini
        if (originalEtablissementClient.DateCreGpi != null)
        {
            patchedEtablissementClient.DateCreGpi = originalEtablissementClient.DateCreGpi;
        }
        
        // Impossible de modifier DateCreOdoo une fois qu'il a été défini
        if (originalEtablissementClient.DateCreOdoo != null)
        {
            patchedEtablissementClient.DateCreOdoo = originalEtablissementClient.DateCreOdoo;
        }
        
        // Impossible de modifier DateCreDashdoc une fois qu'il a été défini
        if (originalEtablissementClient.DateCreDashdoc != null)
        {
            patchedEtablissementClient.DateCreDashdoc = originalEtablissementClient.DateCreDashdoc;
        }
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
    /// Checks whether the user is allowed to update bank information.
    /// </summary>
    /// <returns>
    /// True if the user has the 'write_bank_infos' role claim; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method checks if the user has the claim of the role 'write_bank_infos'.
    /// </remarks>
    public bool CanUserUpdateBankInfo()
    {
        // Vérifier si l'utilisateur a le claim de rôle 'write_bank_infos'
        var hasPermission = _httpContextAccessor.HttpContext.User.HasClaim(c => 
            c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "write_bank_infos");

        return hasPermission;
    }
    
    public async Task<bool> Exists(EtablissementClient obj, ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()   // Pas besoin de tracker les modifications car on ne fait que lire
            .Where(x => x.Id == obj.Id || x.Siret == obj.Siret)
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
    public async Task<EtablissementClient> CreateIfDoesntExistAsync(EtablissementClient etablissementClient, ContextSession session)
    {
        var context = GetContext(session);
        var objectExists = await this.Exists(etablissementClient, session);
        if (objectExists)
        {
            var existingClient = await GetBySiret(etablissementClient.Siret, session);

            // Vérifie directement si la mise à jour de la colonne spécifiée est nécessaire et procède à la mise à jour si c'est le cas
            if (!string.IsNullOrEmpty(this.column))
            {
                var propertyInfo = existingClient.GetType().GetProperty(this.column);
                if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                {
                    bool currentValue = (bool)propertyInfo.GetValue(existingClient);
                    if (!currentValue)
                    {
                        propertyInfo.SetValue(existingClient, true, null);
                        // Exécute la mise à jour directement ici si nécessaire
                        await UpdateAsync(existingClient, session);
                    }
                }
                else
                {
                    throw new ArgumentException($"La propriété '{this.column}' n'existe pas sur le type '{existingClient.GetType().Name}' ou n'est pas de type bool.");
                }
            }
            return existingClient;
        }

        var patchedEtab = SetCreationFilterValues(etablissementClient);

        if(string.IsNullOrEmpty(patchedEtab.CreatedBy))
        {
            patchedEtab.CreatedBy = GetCurrentUserName();
        }
        context.EtablissementClient.Add(patchedEtab);
        await context.SaveChangesAsync();
        return patchedEtab;
    }
    
    /// <summary>
    /// Crée un nouvel établissement client et l'associe à une entreprise et à une fiche d'établissement.
    /// </summary>
    /// <param name="etablissementClient">L'objet EtablissementClient à créer.</param>
    /// <param name="entrepriseBase">L'objet EntrepriseBase associé à l'établissement client.</param>
    /// <param name="etablissementFiche">L'objet EtablissementFiche associé à l'établissement client.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>Retourne l'établissement client créé, ou null si un établissement avec le même identifiant existe déjà.</returns>
    public async Task<EtablissementClient> Create(
        EtablissementClient etablissementClient,
        EntrepriseBase entrepriseBase,
        EtablissementFiche etablissementFiche,
        ContextSession session,
        bool disableTracking = false)
    {
        var context = GetContext(session);
        var clientExists = await GetBySiret(etablissementClient.Siret, session, true);
        if (clientExists != null)
        {
            return await UpdateExistingClient(etablissementClient, etablissementFiche, session, context);
        }

        var patchedEtab = SetCreationFilterValues(etablissementClient);
        if(string.IsNullOrEmpty(patchedEtab.CreatedBy))
        {
            patchedEtab.CreatedBy = GetCurrentUserName();
        }

        var baseExists = await _entrepriseBaseRepository.Exists(entrepriseBase, session);

        if (!baseExists)
        {
            context.EntrepriseBase.Add(entrepriseBase);

            await context.SaveChangesAsync();

            context.Entry(entrepriseBase).State = EntityState.Detached;
        }

        var ficheExists = await context.EtablissementFiche
            .AsNoTracking()
            .AnyAsync(f => f.Siret == etablissementFiche.Siret);

        if (!ficheExists)
        {
            context.EtablissementFiche.Add(etablissementFiche);

            await context.SaveChangesAsync();

            context.Entry(etablissementFiche).State = EntityState.Detached;
        }
        else
        {
            etablissementFiche.IsDeleted = false;
            context.EtablissementFiche.Update(etablissementFiche);

            await context.SaveChangesAsync();

            context.Entry(etablissementFiche).State = EntityState.Detached;
        }

        context.EtablissementClient.Add(patchedEtab);

        await context.SaveChangesAsync();

        context.Entry(patchedEtab).State = EntityState.Detached;

        return patchedEtab;
    }

    private async Task<EtablissementClient> UpdateExistingClient(EtablissementClient etablissementClient,
        EtablissementFiche etablissementFiche, ContextSession session, DataContext context)
    {
        var existingClient = await GetEntities(session, true)
            .FirstOrDefaultAsync(obj => obj.Siret == etablissementClient.Siret);

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
        existingClient.Client = etablissementClient.Client;
        existingClient.Fournisseur = etablissementClient.Fournisseur;

        // Reactivate or update the associated EtablissementFiche
        var existingFiche = await context.EtablissementFiche
            .FirstOrDefaultAsync(f => f.Siret == existingClient.Siret);

        if (existingFiche != null)
        {
            existingFiche.IsDeleted = false;
            UpdateEtablissementFicheFields(existingFiche, etablissementFiche);
        }
        else
        {
            context.EtablissementFiche.Add(etablissementFiche);
        }

        context.Entry(existingClient).State = EntityState.Modified;
        context.Entry(existingFiche).State = EntityState.Modified;

        await context.SaveChangesAsync();

        context.Entry(existingClient).State = EntityState.Detached;
        context.Entry(etablissementFiche).State = EntityState.Detached;
        context.Entry(existingFiche).State = EntityState.Detached;

        return existingClient;
    }

    private static void UpdateEtablissementFicheFields(EtablissementFiche existingFiche, EtablissementFiche etablissementFiche)
    {
        // Update EtablissementFiche fields except Siret and Id
            foreach (PropertyInfo property in typeof(EtablissementFiche).GetProperties())
            {
                // Skip the Siret and Id properties
                if (property.Name == "Siret" || property.Name == "Id")
                    continue;

                // Only copy the value if the property is writable and both properties have the same type
                if (property.CanWrite)
                {
                    var newValue = property.GetValue(etablissementFiche);
                    property.SetValue(existingFiche, newValue);
                }
            }
    }
    
    /// <summary>
    /// Update les informations de l'établissement créé avec un nouveu Siret
    /// </summary>
    /// <param name="etablissementClient"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<EtablissementClient> UpdateWithNewSiretAsync(EtablissementClient etablissementClient, ContextSession session)
    {
        var context = GetContext(session);

        // Detach any existing instance with the same primary key
        var existingEntity = await context.EtablissementClient
            .FirstOrDefaultAsync(e => e.Siret == etablissementClient.Siret);

        if (existingEntity == null)
        {
            throw new Exception("Entity not found");
        }
        
        CopyPropertiesExceptIdAndSiret(etablissementClient, existingEntity);
        
        await context.SaveChangesAsync();

        return etablissementClient;
    }
    
    public async Task<EtablissementClient> DeleteErpCodeAsync(int id, string codeType, ContextSession session)
    {
        var context = GetContext(session);

        var existingEntity = await context.EtablissementClient
            .FirstOrDefaultAsync(e => e.Id == id);

        if (existingEntity == null)
        {
            throw new EntityNotFoundException("Entity not found");
        }

        switch (codeType.ToLower())
        {
            case "mkgt":
                existingEntity.CodeMkgt = null;
                existingEntity.DateCreMKGT = null;
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
    
    private static void CopyPropertiesExceptIdAndSiret(EtablissementClient source, EtablissementClient destination)
    {
        destination.Siren = source.Siren;
        destination.Nom = source.Nom;
        destination.IdOdoo = source.IdOdoo;
        destination.CodeKerlog = source.CodeKerlog;
        destination.CodeMkgt = source.CodeMkgt;
        destination.CodeGpi = source.CodeGpi;
        destination.FrnCodeGpi = source.FrnCodeGpi;
        destination.ContactFacturation = source.ContactFacturation;
        destination.EmailFacturation = source.EmailFacturation;
        destination.TelephoneFacturation = source.TelephoneFacturation;
        destination.PortableFacturation = source.PortableFacturation;
        destination.ContactAlternatif = source.ContactAlternatif;
        destination.EmailAlternatif = source.EmailAlternatif;
        destination.TelephoneAlternatif = source.TelephoneAlternatif;
        destination.PortableAlternatif = source.PortableAlternatif;
        
        // Only overwrite if destination field is empty
        if (string.IsNullOrEmpty(destination.AdresseFacturation1))
            destination.AdresseFacturation1 = source.AdresseFacturation1;
        if (string.IsNullOrEmpty(destination.AdresseFacturation2))
            destination.AdresseFacturation2 = source.AdresseFacturation2;
        if (string.IsNullOrEmpty(destination.AdresseFacturation3))
            destination.AdresseFacturation3 = source.AdresseFacturation3;
        if (string.IsNullOrEmpty(destination.CodePostalFacturation))
            destination.CodePostalFacturation = source.CodePostalFacturation;
        if (string.IsNullOrEmpty(destination.VilleFacturation))
            destination.VilleFacturation = source.VilleFacturation;
        if (string.IsNullOrEmpty(destination.PaysFacturation))
            destination.PaysFacturation = source.PaysFacturation;
        
        destination.ConditionReglement = source.ConditionReglement;
        destination.ModeReglement = source.ModeReglement;
        destination.DelaiReglement = source.DelaiReglement;
        destination.TauxTva = source.TauxTva;
        destination.EncoursMax = source.EncoursMax;
        destination.CompteComptable = source.CompteComptable;
        destination.FrnConditionReglement = source.FrnConditionReglement;
        destination.FrnModeReglement = source.FrnModeReglement;
        destination.FrnDelaiReglement = source.FrnDelaiReglement;
        destination.FrnTauxTva = source.FrnTauxTva;
        destination.FrnEncoursMax = source.FrnEncoursMax;
        destination.FrnCompteComptable = source.FrnCompteComptable;
        destination.Iban = source.Iban;
        destination.Bic = source.Bic;
        destination.ClientBloque = source.ClientBloque;
        destination.MotifBlocage = source.MotifBlocage;
        destination.DateBlocage = source.DateBlocage;
        destination.DateCreMKGT = source.DateCreMKGT;
        destination.DateCreOdoo = source.DateCreOdoo;
        destination.DateCreGpi = source.DateCreGpi;
        destination.Radie = source.Radie;
        destination.IdHubspot = source.IdHubspot;
        destination.IdDashdoc = source.IdDashdoc;
        destination.DateCreDashdoc = source.DateCreDashdoc;
        destination.Client = source.Client;
        destination.Fournisseur = source.Fournisseur;
        destination.CommercialId = source.CommercialId;
        destination.UpdatedAt = source.UpdatedAt;
        destination.UpdatedBy = source.UpdatedBy;
        destination.CreateDate = source.CreateDate;
        destination.CreatedBy = source.CreatedBy;
    }

    /// <summary>
    /// Applies additional column filter to the query.
    /// </summary>
    /// <param name="query">The original query.</param>
    /// <returns>The modified query with column filter applied.</returns>
    private IQueryable<EtablissementClient> ApplyColumnFilter(IQueryable<EtablissementClient> query)
    {
        if (!string.IsNullOrEmpty(column) && typeof(EtablissementClient).GetProperty(column)?.PropertyType == typeof(bool))
        {
            query = query.Where(x => (bool)EF.Property<object>(x, column));
        }

        return query;
    }
    
    private EtablissementClient SetCreationFilterValues(EtablissementClient etablissementClient)
    {
        if (!string.IsNullOrEmpty(column))
        {
            var propertyInfo = etablissementClient.GetType().GetProperty(column);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(etablissementClient, true, null);
            }
            else
            {
                throw new ArgumentException($"La propriété '{column}' n'existe pas sur le type '{etablissementClient.GetType().Name}' ou n'est pas de type bool.");
            }
        }
        return etablissementClient;
    }
}