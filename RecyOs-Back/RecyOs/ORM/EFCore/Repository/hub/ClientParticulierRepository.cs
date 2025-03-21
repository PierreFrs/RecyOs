// ClientParticulierRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using ILogger = NLog.ILogger;

namespace RecyOs.ORM.EFCore.Repository.hub
{
    /// <summary>
    /// Repository for managing ClientParticulier entities with CRUD operations.
    /// </summary>
    public class ClientParticulierRepository : BaseDeletableRepository<ClientParticulier, DataContext>, IClientParticulierRepository
    {
        private readonly ITokenInfoService _tokenInfoService;
        private readonly ILogger<ClientParticulierRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the ClientParticulierRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="tokenInfoService">The token info service for retrieving user information.</param>
        /// <param name="logger">The logger service.</param>
        public ClientParticulierRepository(
            DataContext context, 
            ITokenInfoService tokenInfoService, 
            ILogger<ClientParticulierRepository> logger) : base(context)
        {
            _tokenInfoService = tokenInfoService;
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously creates a new ClientParticulier.
        /// </summary>
        /// <param name="clientParticulier">The client to be added.</param>
        /// <param name="session">The current context session.</param>
        /// <returns>The created ClientParticulier entity.</returns>
        public async Task<ClientParticulier> CreateClientParticulierAsync(ClientParticulier clientParticulier, ContextSession session)
        {
            var context = GetContext(session);

            try
            {
                if (await Exists(clientParticulier, session))
                {
                    throw new InvalidOperationException("A client with the same name and surname already exists.");
                }

                clientParticulier.CreateDate = DateTime.Now;
                clientParticulier.CreatedBy = _tokenInfoService.GetCurrentUserName();

                var addedClient = await context.ClientParticuliers.AddAsync(clientParticulier);
                await context.SaveChangesAsync();

                return addedClient.Entity;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation(ex, "An error occurred while creating the client in the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves all instances of <see cref="ClientParticulier"/> from the database.
        /// </summary>
        /// <param name="session">The context session.</param>
        /// <param name="includeDeleted">A flag indicating whether to include deleted instances.</param>
        /// <returns>An array of <see cref="ClientParticulier"/>.</returns>
        public async Task<IList<ClientParticulier>> GetAll(ContextSession session, bool includeDeleted = false)
        {
            IQueryable<ClientParticulier> query = GetEntities(session, includeDeleted);

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a filtered list of ClientParticulier entities and the count based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to be applied to the query.</param>
        /// <param name="session">The current context session.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <returns>A tuple containing the filtered list of clients and the total count.</returns>
        public async Task<(IEnumerable<ClientParticulier>, int)> GetFilteredListWithCountAsync(ClientParticulierGridFilter filter, ContextSession session, bool includeDeleted = false)
        {
            try
            {
                var query = GetEntities(session, includeDeleted)
                    .ApplyFilter(filter);

                if (filter.PageSize <= 0 || filter.PageNumber < 0)
                {
                    throw new InvalidOperationException("PageSize and PageNumber must be positive values.");
                }

                var count = await query.CountAsync();
                var clients = await query
                    .Skip(filter.PageSize * filter.PageNumber)
                    .Take(filter.PageSize)
                    .AsNoTracking()
                    .ToArrayAsync();

                return (clients, count);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation(ex, "An error occurred while retrieving the client list from the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a ClientParticulier by its identifier.
        /// </summary>
        /// <param name="id">The client identifier.</param>
        /// <param name="session">The current context session.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <returns>The requested ClientParticulier entity or null if not found.</returns>
        public async Task<ClientParticulier> GetClientParticulierByIdAsync(int id, ContextSession session, bool includeDeleted = false)
        {
            try
            {
                if (includeDeleted)
                {
                    return await GetEntities(session, includeDeleted)
                        .Where(x => x.Id == id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }
                var clientExists = await GetEntities(session, includeDeleted)
                    .Where(x => x.Id == id)
                    .AnyAsync();
                
                if (!clientExists)
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }
                
                var client = await GetEntities(session, includeDeleted)
                    .Where(x => x.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                
                return client;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while retrieving the client from the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a ClientParticulier by its name.
        /// </summary>
        /// <param name="prenom">The client first name.</param>
        /// <param name="nom">The client last name.</param>
        /// <param name="ville">La ville du client particulier recherché</param>
        /// <param name="session">The current context session.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <returns>The requested ClientParticulier entity or null if not found.</returns>
        public async Task<ClientParticulier> GetClientParticulierByNameAndCityAsync(string prenom, string nom, string ville, ContextSession session, bool includeDeleted = false)
        {
            try
            {
                if (includeDeleted)
                {
                    return await GetEntities(session, includeDeleted)
                        .Where(x => x.Prenom == prenom && x.Nom == nom && x.VilleFacturation == ville)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }

                var client = await GetEntities(session, includeDeleted)
                    .Where(x => x.Prenom == prenom && x.Nom == nom && x.VilleFacturation == ville)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (client == null)
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }

                return client;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while retrieving the client from the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a ClientParticulier by its name.
        /// </summary>
        /// <param name="codeMkgt">The client MKGT code.</param>
        /// <param name="session">The current context session.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
        /// <returns>The requested ClientParticulier entity or null if not found.</returns>
        public async Task<ClientParticulier> GetClientParticulierByCodeMkgtAsync(string codeMkgt, ContextSession session, bool includeDeleted = false)
        {
            try
            {
                if (includeDeleted)
                {
                    return await GetEntities(session, includeDeleted)
                        .Where(x => x.CodeMkgt == codeMkgt)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }
                var clientExists = await GetEntities(session, includeDeleted)
                    .Where(x => x.CodeMkgt == codeMkgt)
                    .AnyAsync();

                if (!clientExists)
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }

                var client = await GetEntities(session, includeDeleted)
                    .Where(x => x.CodeMkgt == codeMkgt)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return client;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while retrieving the client from the repository.");
                throw ex;
            }
        }


        /// <summary>
        /// Asynchronously updates an existing ClientParticulier.
        /// </summary>
        /// <param name="id">The client identifier.</param>
        /// <param name="clientParticulier">The updated ClientParticulier object.</param>
        /// <param name="session">The current context session.</param>
        /// <returns>The updated ClientParticulier entity.</returns>
        public async Task<ClientParticulier> UpdateClientParticulierAsync(int id, ClientParticulier clientParticulier, ContextSession session)
        {
            var context = GetContext(session);

            try
            {
                if (!await Exists(clientParticulier, session))
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }

                var originalClient = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                clientParticulier.UpdatedAt = DateTime.Now;
                clientParticulier.UpdatedBy = _tokenInfoService.GetCurrentUserName();

                // Traitement des champs qui ne sont pas en réecriture libre
                if (originalClient.CodeMkgt != null && clientParticulier.CodeMkgt == string.Empty)
                {
                    clientParticulier.CodeMkgt = originalClient.CodeMkgt;
                }

                if (originalClient.IdOdoo != null && clientParticulier.IdOdoo == string.Empty)
                {
                    clientParticulier.IdOdoo = originalClient.IdOdoo;
                }

                if (originalClient.IdShipperDashdoc != null && clientParticulier.IdShipperDashdoc == null)
                {
                    clientParticulier.IdShipperDashdoc = originalClient.IdShipperDashdoc;
                }

                // Renseignement des dates de création MKGT et Odoo
                if (!string.IsNullOrEmpty(clientParticulier.CodeMkgt))
                {
                    clientParticulier.DateCreMkgt = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(clientParticulier.IdOdoo))
                {
                    clientParticulier.DateCreOdoo = DateTime.Now;
                }

                context.Entry(originalClient).CurrentValues.SetValues(clientParticulier);
                context.Entry(originalClient).Property(x => x.CreatedBy).IsModified = false;
                context.Entry(originalClient).Property(x => x.CreateDate).IsModified = false;
                await context.SaveChangesAsync();

                await context.Entry(originalClient).ReloadAsync();

                return originalClient;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while updating the client in the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Asynchronously performs a soft delete of a ClientParticulier by marking it as deleted.
        /// </summary>
        /// <param name="id">The client identifier.</param>
        /// <param name="session">The current context session.</param>
        /// <returns>A boolean indicating if the client was successfully deleted.</returns>
        public async Task<bool> SoftDeleteClientParticulierAsync(int id, ContextSession session)
        {
            var context = GetContext(session);
            try
            {
                var client = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .AnyAsync();

                if (!client)
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }

                await Delete(id, session);

                var isDeleted = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .Select(x => x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (!isDeleted)
                {
                    throw new ArgumentException("An error occurred while deleting the client.");
                }

                return isDeleted;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
                throw ex;
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
                throw ex;
            }
        }

        /// <summary>
        /// Asynchronously performs a hard delete of a ClientParticulier by removing it from the database.
        /// </summary>
        /// <param name="id">The client identifier.</param>
        /// <param name="session">The current context session.</param>
        /// <returns>A boolean indicating if the client was successfully deleted.</returns>
        public async Task<bool> HardDeleteClientParticulierAsync(int id, ContextSession session)
        {
            var context = GetContext(session);
            try
            {
                var client = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .AnyAsync();

                if (!client)
                {
                    throw new EntityNotFoundException("The provided client does not yet exist in the database, please proceed to creation.");
                }

                var clientToDelete = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                context.ClientParticuliers.Remove(clientToDelete);
                await context.SaveChangesAsync();

                var isntDeleted = await context.ClientParticuliers
                    .Where(x => x.Id == id)
                    .AnyAsync();

                if (isntDeleted)
                {
                    throw new ArgumentException("An error occurred while deleting the client.");
                }

                return true;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
                throw ex;
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex, "An error occurred while deleting the client in the repository.");
                throw ex;
            }
        }

        public async Task<ClientParticulier> DeleteErpCodeAsync(int id, string codeType, ContextSession session)
        {
            var context = GetContext(session);

            var existingEntity = await context.ClientParticuliers
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEntity == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }

            switch (codeType.ToLower())
            {
                case "mkgt":
                    existingEntity.CodeMkgt = null;
                    existingEntity.DateCreMkgt = null;
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
}
