using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Interfaces;

public interface ISocieteRepository
{
    /// <summary>
    /// Récupère une société par son identifiant
    /// </summary>
    /// <param name="id">L'identifiant de la société</param>
    /// <param name="includeDeleted">Inclure les éléments supprimés</param>
    /// <returns>La société correspondante ou null si non trouvée</returns>
    Task<Societe?> GetByIdAsync(int id, bool includeDeleted = false);

    /// <summary>
    /// Récupère une société par son identifiant Odoo
    /// </summary>
    /// <param name="idOdoo">L'identifiant Odoo de la société</param>
    /// <param name="includeDeleted">Inclure les éléments supprimés</param>
    /// <returns>La société correspondante ou null si non trouvée</returns>
    Task<Societe?> GetByIdOdooAsync(string idOdoo, bool includeDeleted = false);

    /// <summary>
    /// Récupère toutes les sociétés
    /// </summary>
    /// <param name="includeDeleted">Inclure les éléments supprimés</param>
    /// <returns>Liste des sociétés</returns>
    Task<IEnumerable<Societe>> GetAllAsync(bool includeDeleted = false);

    /// <summary>
    /// Crée une nouvelle société
    /// </summary>
    /// <param name="societe">La société à créer</param>
    /// <returns>La société créée avec son ID</returns>
    Task<Societe> CreateAsync(Societe societe);

    /// <summary>
    /// Met à jour une société existante
    /// </summary>
    /// <param name="societe">La société à mettre à jour</param>
    /// <returns>La société mise à jour</returns>
    Task<Societe?> UpdateAsync(Societe societe);

    /// <summary>
    /// Supprime une société (soft delete)
    /// </summary>
    /// <param name="id">L'identifiant de la société à supprimer</param>
    /// <returns>True si la suppression a réussi, False sinon</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Vérifie si une société existe
    /// </summary>
    /// <param name="id">L'identifiant de la société</param>
    /// <param name="includeDeleted">Inclure les éléments supprimés</param>
    /// <returns>True si la société existe, False sinon</returns>
    Task<bool> ExistsAsync(int id, bool includeDeleted = false);

    /// <summary>
    /// Vérifie si une société existe avec cet ID Odoo
    /// </summary>
    /// <param name="idOdoo">L'identifiant Odoo de la société</param>
    /// <param name="includeDeleted">Inclure les éléments supprimés</param>
    /// <returns>True si la société existe, False sinon</returns>
    Task<bool> ExistsByIdOdooAsync(string idOdoo, bool includeDeleted = false);
}
