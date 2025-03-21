using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IClientParticulierService
{
    /// <summary>
    /// Crée un client particulier.
    /// </summary>
    /// <param name="clientParticulierDto">L'objet ClientParticulierDto à ajouter.</param>
    /// <returns>L'objet ClientParticulierDto ajouté.</returns>
    Task<ClientParticulierDto> CreateClientParticulierAsync(ClientParticulierDto clientParticulierDto);
    
    /// <summary>
    /// Récupère une liste filtrée de clients particuliers et le compte total correspondant à ce filtre.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la liste de clients.</param>
    /// <param name="includeDeleted">Indique si les clients supprimés doivent être inclus.</param>
    /// <returns>Un tuple contenant une liste de clients DTO et le compte total correspondant au filtre.</returns>
    Task<GridData<ClientParticulierDto>> GetFilteredListWithCountAsync(ClientParticulierGridFilter filter, bool includeDeleted = false);

    /// <summary>
    /// Retrieves all ClientParticulierDto objects from the database.
    /// </summary>
    /// <param name="includeDeleted">Indicates whether to include deleted records in the result. Default is false.</param>
    /// <returns>An array of ClientParticulierDto objects.</returns>
    Task<IList<ClientParticulierDto>> GetAll(bool includeDeleted = false);

    /// <summary>
    /// Récupère un client particulier par son identifiant unique.
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à récupérer.</param>
    /// <param name="includeDeleted">Indique si les clients supprimés doivent être inclus dans la recherche.</param>
    /// <returns>L'objet ClientParticulierDto correspondant à l'identifiant, ou null si aucun client n'est trouvé.</returns>
    Task<ClientParticulierDto> GetClientParticulierByIdAsync(int id, bool includeDeleted = false);

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="prenom">The client first name.</param>
    /// <param name="nom">The client last name.</param>
    /// <param name="ville">La ville du client particulier recherché</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier DTO or null if not found.</returns>
    Task<ClientParticulierDto> GetClientParticulierByNameAndCityAsync(string prenom, string nom, string ville, bool includeDeleted = false);

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="codeMkgt">The client MKGT code.</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier DTO or null if not found.</returns>
    Task<ClientParticulierDto> GetClientParticulierByCodeMkgtAsync(string codeMkgt, bool includeDeleted = false);

    /// <summary>
    /// Met à jour les informations d'un client particulier existant.
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à mettre à jour.</param>
    /// <param name="clientParticulierDto">L'objet ClientParticulierDto contenant les nouvelles informations.</param>
    /// <returns>L'objet ClientParticulierDto mis à jour.</returns>
    Task<ClientParticulierDto> UpdateClientParticulierAsync(int id, ClientParticulierDto clientParticulierDto);

    /// <summary>
    /// Marque un client particulier comme supprimé sans le retirer physiquement de la base de données (soft delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à marquer comme supprimé.</param>
    /// <returns>Une tâche représentant l'opération de suppression logique.</returns>
    Task<bool> SoftDeleteClientParticulierAsync(int id);

    /// <summary>
    /// Supprime définitivement un client particulier de la base de données (hard delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à supprimer.</param>
    /// <returns>Une tâche représentant l'opération de suppression définitive.</returns>
    Task<bool> HardDeleteClientParticulierAsync(int id);

    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id">L'identifiant du client.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne une entitée ServiceResult contenant le DTO du client après modification ou le message .</returns>
    Task<ClientParticulierDto> DeleteErpCodeAsync(int id, string codeType);
}
