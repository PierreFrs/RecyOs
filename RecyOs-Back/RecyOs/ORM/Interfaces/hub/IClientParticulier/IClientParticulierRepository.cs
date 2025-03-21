// IClientParticulierRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IClientParticulierRepository
{
    /// <summary>
    /// Crée un client particulier.
    /// </summary>
    /// <param name="clientParticulier">L'objet ClientParticulier à ajouter à la base de données.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>L'objet ClientParticulier ajouté à la base de données.</returns>
    Task<ClientParticulier> CreateClientParticulierAsync(ClientParticulier clientParticulier, ContextSession session);
    
    /// <summary>
    /// Récupère une liste filtrée de clients particuliers et le compte total correspondant à ce filtre.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la liste de clients.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les clients supprimés doivent être inclus.</param>
    /// <returns>Un tuple contenant une liste de clients et le compte total correspondant au filtre.</returns>
    Task<(IEnumerable<ClientParticulier>, int)> GetFilteredListWithCountAsync(ClientParticulierGridFilter filter, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves all the instances of ClientParticulier from the database.
    /// </summary>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Indicates whether deleted ClientParticulier instances should be included.</param>
    /// <returns>An array of ClientParticulier instances.</returns>
    Task<IList<ClientParticulier>> GetAll(ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Récupère un client particulier par son identifiant unique.
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à récupérer.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les clients supprimés doivent être inclus dans la recherche.</param>
    /// <returns>L'objet ClientParticulier correspondant à l'identifiant, ou null si aucun client n'est trouvé.</returns>
    Task<ClientParticulier> GetClientParticulierByIdAsync(int id, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="prenom">The client first name.</param>
    /// <param name="nom">The client last name.</param>
    /// <param name="ville">La ville du client particulier recherché</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier entity or null if not found.</returns>
    Task<ClientParticulier> GetClientParticulierByNameAndCityAsync(string prenom, string nom, string ville, ContextSession session,
        bool includeDeleted = false);

    /// <summary>
    /// Asynchronously retrieves a ClientParticulier by its name.
    /// </summary>
    /// <param name="codeMkgt">The client MKGT code.</param>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Whether to include soft-deleted entities.</param>
    /// <returns>The requested ClientParticulier entity or null if not found.</returns>
    Task<ClientParticulier> GetClientParticulierByCodeMkgtAsync(string codeMkgt, ContextSession session,
        bool includeDeleted = false);

    /// <summary>
    /// Met à jour les informations d'un client particulier existant.
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à mettre à jour.</param>
    /// <param name="clientParticulier">L'objet ClientParticulier contenant les nouvelles informations.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>L'objet ClientParticulier mis à jour.</returns>
    Task<ClientParticulier> UpdateClientParticulierAsync(int id, ClientParticulier clientParticulier, ContextSession session);

    /// <summary>
    /// Marque un client particulier comme supprimé sans le retirer physiquement de la base de données (soft delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à marquer comme supprimé.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>Une tâche représentant l'opération de suppression logique.</returns>
    Task<bool> SoftDeleteClientParticulierAsync(int id, ContextSession session);

    /// <summary>
    /// Supprime définitivement un client particulier de la base de données (hard delete).
    /// </summary>
    /// <param name="id">L'identifiant du client particulier à supprimer.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <returns>Une tâche représentant l'opération de suppression définitive.</returns>
    Task<bool> HardDeleteClientParticulierAsync(int id, ContextSession session);

    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="codeType"></param>
    /// <param name="session"></param>
    /// <returns>ClientParticulier</returns>
    Task<ClientParticulier> DeleteErpCodeAsync(int id, string codeType, ContextSession session);

}