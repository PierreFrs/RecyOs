using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEuropeService
{
    /// <summary>
    /// Récupère les données filtrées pour une grille clients européens.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un objet contenant les éléments clients correspondant au filtrage et les informations de pagination.</returns>
    Task<GridData<ClientEuropeDto>> GetGridDataAsync(ClientEuropeGridFilter filter, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un client Européen par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant à l'identifiant donné.</returns>
    Task<ClientEuropeDto> GetById(int id, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un client Européen par son code TVA.
    /// </summary>
    /// <param name="vat">Le code TVA du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code TVA donné.</returns>
    Task<ClientEuropeDto> GetByVat(string vat, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un client Européen par son code Kerlog.
    /// </summary>
    /// <param name="codeKerlog">Le code Kerlog du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code Kerlog donné.</returns>
    Task<ClientEuropeDto> GetByCodeKerlog(string codeKerlog, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un client Européen par son code MKGT.
    /// </summary>
    /// <param name="codeMkgt">Le code MKGT du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant au code MKGT donné.</returns>
    Task<ClientEuropeDto> GetByCodeMkgt(string codeMkgt, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un client Européen par son ID Odoo.
    /// </summary>
    /// <param name="idOdoo">L'ID Odoo du client européen recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO du client européen correspondant à l'ID Odoo donné.</returns>
    Task<ClientEuropeDto> GetByIdOdoo(string idOdoo, bool includeDeleted = false);

    /// <summary>
    /// Récupère le groupe d'un client européen par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen.</param>
    /// <returns>Retourne le groupe du client européen.</returns>
    Task<GroupDto> GetGroup(int id);
    
    /// <summary>
    /// Supprime un client Européen en fonction de son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du client européen à supprimer.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    Task<bool> DeleteAsync(int id, bool estClient, bool estFournisseur);
    
    /// <summary>
    /// Modifie un client Européen à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations à jour du client européen.</param>
    /// <returns>Retourne le client européen mis à jour.</returns>
    /// <remarks>Le numéro de TVA ne peut pas être modifié.</remarks>
    Task<ClientEuropeDto> Update(ClientEuropeDto dto);
    
    /// <summary>
    /// Crée un nouveau client Européen à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations du client européen à créer.</param>
    /// <param name="disableTracking"></param>
    /// <returns>Retourne le client européen créé.</returns>
    Task<ClientEuropeDto> Create(ClientEuropeDto dto);

    /// <summary>
    /// Retrieves all ClientEuropeDto objects from the database.
    /// </summary>
    /// <param name="includeDeleted">Indicates whether to include deleted records in the result. Default is false.</param>
    /// <returns>An array of ClientEuropeDto objects.</returns>
    Task<IList<ClientEuropeDto>> GetAll(bool includeDeleted = false);

    /// <summary>
    /// Crée un nouvel etablissement europeen contenant uniqument un code TVA.
    /// </summary>
    /// <param name="vat">Le code TVA de l'etablissement.</param>
    /// <returns>Retourne le client européen créé.</returns>
    Task<ClientEuropeDto> CreateFromScratchAsync(string vat);
    
    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client européen.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne une entitée ServiceResult contenant le DTO de l'établissement client européen après modification ou le message .</returns>
    Task<ClientEuropeDto> DeleteErpCodeAsync(int id, string codeType);
}