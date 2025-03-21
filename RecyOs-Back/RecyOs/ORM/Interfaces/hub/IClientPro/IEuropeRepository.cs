using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEuropeRepository<TClientEurope> where TClientEurope : ClientEurope
{
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
    Task<TClientEurope> GetById(int id, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves a ClientEurope entity by its VAT number.
    /// </summary>
    /// <param name="vat">The VAT number of the client.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">Optional flag to include deleted entities. Default is false.</param>
    /// <returns>The ClientEurope entity with the specified VAT number, or null if not found.</returns>
    Task<TClientEurope> GetByVat(string vat, ContextSession session, bool includeDeleted = false);

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
    Task<TClientEurope> GetByCodeKerlog(string codeKerlog, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves a filtered list of ClientEurope entities along with the total count.
    /// </summary>
    /// <param name="filter">The filter to apply to the query.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">A flag indicating whether to include deleted entities.</param>
    /// <returns>
    /// A tuple containing the filtered list of ClientEurope entities and the total count.
    /// </returns>
    Task<(IEnumerable<TClientEurope>, int)> GetFiltredListWithCount(ClientEuropeGridFilter filter,
        ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves a <see cref="ClientEurope"/> entity based on the specified <paramref name="codeMkgt"/>.
    /// </summary>
    /// <param name="codeMkgt">The code value of the entity.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">True to include deleted entities, false otherwise.</param>
    /// <returns>The <see cref="ClientEurope"/> entity matching the specified <paramref name="codeMkgt"/>, or null if not found.</returns>
    Task<TClientEurope> GetByCodeMkgt(string codeMkgt, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves a ClientEurope entity by its Odoo ID.
    /// </summary>
    /// <param name="idOdoo">The Odoo ID of the ClientEurope entity.</param>
    /// <param name="session">The ContextSession instance.</param>
    /// <param name="includeDeleted">Whether to include deleted entities. Default is false.</param>
    /// <returns>The ClientEurope entity with the specified Odoo ID, or null if not found.</returns>
    Task<TClientEurope> GetByIdOdoo(string idOdoo, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Updates a <see cref="ClientEurope"/> entity.
    /// </summary>
    /// <param name="clientEurope">The <see cref="ClientEurope"/> entity to update.</param>
    /// <param name="session">The <see cref="ContextSession"/> object.</param>
    /// <returns>The updated <see cref="ClientEurope"/> entity.</returns>
    Task<TClientEurope> UpdateAsync(TClientEurope clientEurope, ContextSession session);

    /// <summary>
    /// Check if a particular clientEurope exists in the database based on its id or vat.
    /// </summary>
    /// <param name="clientEurope">The clientEurope to check existence for.</param>
    /// <param name="session">The session used for the database connection.</param>
    /// <returns>True if the clientEurope exists, False otherwise.</returns>
    Task<bool> Exists(TClientEurope clientEurope, ContextSession session);

    /// <summary>
    /// Creates a new instance of ClientEurope if it doesn't already exist in the database. If it does exist,
    /// updates a specified column's value to true and returns the updated object.
    /// </summary>
    /// <param name="clientEurope">The ClientEurope object to create or update</param>
    /// <param name="session">The context session</param>
    /// <param name="disableTracking"></param>
    /// <returns>
    /// The created ClientEurope object if it was newly created, or the updated ClientEurope object
    /// if it already existed and its column value was updated.
    /// </returns>
    Task<TClientEurope> CreateAsync(TClientEurope clientEurope, ContextSession session);

    /// <summary>
    /// Retrieves all the instances of TClientEurope from the database.
    /// </summary>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Indicates whether deleted TClientEurope instances should be included.</param>
    /// <returns>An array of TClientEurope instances.</returns>
    Task<IList<TClientEurope>> GetAll(ContextSession session, bool includeDeleted = false);


    /// <summary>
    /// Delete a client's establishment.
    /// </summary>
    /// <param name="id">The ID of the establishment to delete.</param>
    /// <param name="session">The session context.</param>
    /// <returns>A task representing the deletion operation.</returns>
    Task Delete(int id, ContextSession session);
    
    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="codeType"></param>
    /// <param name="session"></param>
    /// <returns>ClientEurope</returns>
    Task<TClientEurope> DeleteErpCodeAsync(int id, string codeType, ContextSession session);
}