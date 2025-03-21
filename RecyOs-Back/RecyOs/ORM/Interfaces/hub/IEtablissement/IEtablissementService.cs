using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementService
{
    /// <summary>
    /// Récupère les données filtrées pour une grille d'établissements clients.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un objet contenant les éléments de l'établissement client correspondant au filtrage et les informations de pagination.</returns>
    Task<GridData<EtablissementClientDto>> GetDataForGrid(EtablissementClientGridFilter filter, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant à l'identifiant donné.</returns>
    Task<EtablissementClientDto> GetById(int id, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client par son numéro SIRET.
    /// </summary>
    /// <param name="siret">Le numéro SIRET de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au numéro SIRET donné.</returns>
    Task<EtablissementClientDto> GetBySiret(string siret, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client par son code Kerlog.
    /// </summary>
    /// <param name="codeKerlog">Le code Kerlog de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au code Kerlog donné.</returns>
    Task<EtablissementClientDto> GetByCodeKerlog(string codeKerlog, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client par son code MKGT.
    /// </summary>
    /// <param name="codeMkgt">Le code MKGT de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant au code MKGT donné.</returns>
    Task<EtablissementClientDto> GetByCodeMkgt(string codeMkgt, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client par son ID Odoo.
    /// </summary>
    /// <param name="idOdoo">L'ID Odoo de l'établissement client recherché.</param>
    /// <param name="includeDeleted">Indique si les éléments supprimés doivent être inclus. Par défaut, ils ne le sont pas.</param>
    /// <returns>Retourne un DTO de l'établissement client correspondant à l'ID Odoo donné.</returns>
    Task<EtablissementClientDto> GetByIdOdoo(string idOdoo, bool includeDeleted = false);

    /// <summary>
    /// Récupère le groupe associé à un établissement client.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <returns>Retourne le groupe associé à l'établissement client.</returns>
    Task<GroupDto> GetGroup(int id);

    /// <summary>
    /// Récupère une liste d'établissement partageant un même SIREN.
    /// </summary>
    /// <param name="siren">Le SIREN des établissements recherchés.</param>
    /// <returns>Retourne une liste de DTO des établissements partageant le même SIREN.</returns>
    Task<IEnumerable<EtablissementClientDto>> GetEtablissementGroupBySirenAsync(string siren);

    /// <summary>
    /// Supprime un établissement
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement à supprimer</param>
    /// <param name="estClient">Indique si c'est un client</param>
    /// <param name="estFournisseur">Indique si c'est un fournisseur</param>
    /// <returns>True si la suppression a réussi</returns>
    Task<bool> DeleteAsync(int id, bool estClient, bool estFournisseur);
    
    /// <summary>
    /// Modifie un établissement client à partir d'un DTO fourni.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations à jour de l'établissement client.</param>
    /// <returns>Retourne un DTO de l'établissement client après modification.</returns>
    Task<EtablissementClientDto> Edit(EtablissementClientDto dto);
    
    /// <summary>
    /// Modifie le Siret d'un établissement client à partir d'une string fourni.
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <param name="newSiret">Le siret de l'établissement client</param>
    /// <param name="estClient">la fiche est un client</param>
    /// <param name="estFournisseur">la fiche est un fournisseur</param>
    /// <param name="session"></param>
    /// <returns>Retourne une entitée ServiceResult contenant le DTO de l'établissement client après modification ou le message .</returns>
    Task<ServiceResult> ChangeSiretAsync(int id, string newSiret, bool estClient, bool estFournisseur, ContextSession session);
    
    /// <summary>
    /// Crée un nouvel établissement client à partir d'un DTO et l'associe à une entreprise et à une fiche d'établissement, également fournies sous forme de DTO.
    /// </summary>
    /// <param name="dto">Le DTO représentant l'établissement client à créer.</param>
    /// <param name="entrepriseBaseDto">Le DTO représentant l'entreprise de base associée à l'établissement client.</param>
    /// <param name="etablissementFicheDto">Le DTO représentant la fiche d'établissement associée à l'établissement client.</param>
    /// <param name="disableTracking"></param>
    /// <returns>Retourne un DTO de l'établissement client créé.</returns>
    Task<EtablissementClientDto> Create(EtablissementClientDto dto, EntrepriseBaseDto entrepriseBaseDto,
        EtablissementFicheDto etablissementFicheDto, bool disableTracking);
    
    /// <summary>
    /// Crée un nouvel établissement client dans la base de données à partir d'un DTO.
    /// Si l'établissement client existe déjà, il ne sera ni créé, ni mis à jour.
    /// </summary>
    /// <param name="dto">Le Data Transfer Object représentant l'établissement client.</param>
    /// <returns>Le DTO de l'établissement client créé.</returns>
    Task<EtablissementClientDto> Create(EtablissementClientDto dto);

    /// <summary>
    /// Retrieves all EtablissementClientDto objects from the database.
    /// </summary>
    /// <param name="includeDeleted">Indicates whether to include deleted records in the result. Default is false.</param>
    /// <returns>An array of EtablissementClientDto objects.</returns>
    Task<IList<EtablissementClientDto>> GetAll(bool includeDeleted = false);
    
    /// <summary>
    /// Crée un nouvel établissement contenant uniquement un siret
    /// </summary>
    /// <param name="siret">L'identifiant administratif de l'établissement</param>
    /// <returns>Le DTO de l'établissement créé</returns>
    Task<EtablissementClientDto> CreateFromScratchAsync(string siret);
    
    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id">L'identifiant de l'établissement client.</param>
    /// <param name="codeType">Le type de code à supprimer.</param>
    /// <returns>Retourne une entitée ServiceResult contenant le DTO de l'établissement client après modification ou le message .</returns>
    Task<EtablissementClientDto> DeleteErpCodeAsync(int id, string codeType);
}
