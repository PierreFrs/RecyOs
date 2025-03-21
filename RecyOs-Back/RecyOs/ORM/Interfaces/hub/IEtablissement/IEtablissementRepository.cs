using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementRepository<TEtablissementClient> where TEtablissementClient : EtablissementClient
{
    Task Delete(int id, ContextSession session);
    
    /// <summary>
    /// Récupère une liste filtrée d'établissements clients et le compte total correspondant à ce filtre.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la liste d'établissements clients.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus.</param>
    /// <returns>Un tuple contenant une liste d'établissements clients et le compte total correspondant au filtre.</returns>
    Task<(IEnumerable<TEtablissementClient>,int)> GetFiltredListWithCount(EtablissementClientGridFilter filter, ContextSession session, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client en utilisant son SIRET.
    /// </summary>
    /// <param name="siret">Le SIRET de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche.</param>
    /// <returns>L'établissement client correspondant au SIRET fourni, ou null s'il n'est pas trouvé.</returns>
    Task<TEtablissementClient> GetBySiret(string siret, ContextSession session, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client basé sur son code Kerlog.
    /// </summary>
    /// <param name="codeKerlog">Le code Kerlog de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche. Par défaut à false.</param>
    /// <returns>L'établissement client correspondant au code Kerlog fourni, ou null si aucun établissement n'a été trouvé.</returns>
    Task<TEtablissementClient> GetByCodeKerlog(string codeKerlog, ContextSession session, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client basé sur son code Mkgt.
    /// </summary>
    /// <param name="codeMkgt">Le code Mkgt de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche. Par défaut à false.</param>
    /// <returns>L'établissement client correspondant au code Mkgt fourni, ou null si aucun établissement n'a été trouvé.</returns>
    Task<TEtablissementClient> GetByCodeMkgt(string codeMkgt, ContextSession session, bool includeDeleted = false);
    
    /// <summary>
    /// Récupère un établissement client basé sur son identifiant Odoo.
    /// </summary>
    /// <param name="idOdoo">L'identifiant Odoo de l'établissement client à rechercher.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="includeDeleted">Indique si les établissements clients supprimés doivent être inclus dans la recherche. Par défaut à false.</param>
    /// <returns>L'établissement client correspondant à l'identifiant Odoo fourni, ou null si aucun établissement n'a été trouvé.</returns>
    Task<TEtablissementClient> GetByIdOdoo(string idOdoo, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves an instance of EtablissementClient by its GPI code.
    /// </summary>
    /// <param name="codeGpi">The GPI code of the etablissement.</param>
    /// <param name="session">The context session.</param>
    /// <param name="includeDeleted">Whether to include deleted etablissements in the search (default is false).</param>
    /// <returns>An instance of EtablissementClient matching the GPI code.</returns>
    Task<TEtablissementClient> GetByCodeGpi(string codeGpi, ContextSession session, bool includeDeleted = false);
    Task<TEtablissementClient> Get(int id, ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves an <see cref="EtablissementClient"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="EtablissementClient"/> to retrieve.</param>
    /// <param name="session">The <see cref="ContextSession"/> for the current session.</param>
    /// <param name="includeDeleted">Specifies whether to include deleted <see cref="EtablissementClient"/> instances in the result. Default is <c>false</c>.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation that returns the retrieved <see cref="EtablissementClient"/> instance.
    /// </returns>
    Task<TEtablissementClient> GetById(int id, ContextSession session, bool includeDeleted = false);
    
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
    Task<TEtablissementClient> UpdateAsync(TEtablissementClient etablissementClient, ContextSession session);
    
    Task<IEnumerable<TEtablissementClient>> GetList(ContextSession session, bool includeDeleted = false);
    
    /// <summary>
    /// Crée un nouvel établissement client et l'associe à une entreprise et à une fiche d'établissement.
    /// </summary>
    /// <param name="etablissementClient">L'objet EtablissementClient à créer.</param>
    /// <param name="entrepriseBase">L'objet EntrepriseBase associé à l'établissement client.</param>
    /// <param name="etablissementFiche">L'objet EtablissementFiche associé à l'établissement client.</param>
    /// <param name="session">La session de contexte actuelle.</param>
    /// <param name="disableTracking"></param>
    /// <returns>Retourne l'établissement client créé, ou null si un établissement avec le même identifiant existe déjà.</returns>
    Task<TEtablissementClient> Create(
        EtablissementClient etablissementClient,
        EntrepriseBase entrepriseBase,
        EtablissementFiche etablissementFiche,
        ContextSession session,
        bool disableTracking = false);

    /// <summary>
    /// Retrieves all the instances of TEtablissementClient from the database.
    /// </summary>
    /// <param name="session">The current context session.</param>
    /// <param name="includeDeleted">Indicates whether deleted TEtablissementClient instances should be included.</param>
    /// <returns>An array of TEtablissementClient instances.</returns>
    Task<IEnumerable<TEtablissementClient>> GetAll(ContextSession session, bool includeDeleted = false);

    /// <summary>
    /// Retrieves all EtablissementClient sharing the same siren from the database.
    /// </summary>
    /// <param name="siren">The siren of the EtablissementClient to retrieve.</param>
    /// <returns>A List of EtablissementClient sharing the same siren.</returns>
    Task<IEnumerable<TEtablissementClient>> GetEtablissementGroupBySirenAsync(
        string siren,
        ContextSession session,
        bool includeDeleted = false);

    Task<TEtablissementClient> CreateIfDoesntExistAsync(EtablissementClient etablissementClient, ContextSession session);

    /// <summary>
    /// Update les informations de l'établissement créé avec un nouveu Siret
    /// </summary>
    /// <param name="etablissementClient">L'objet EtablissementClient à mettre à jour</param>
    /// <param name="session"></param>
    /// <returns>Retourne l'établissement client mis à jour</returns>
    Task<TEtablissementClient> UpdateWithNewSiretAsync(EtablissementClient etablissementClient, ContextSession session);
    
    /// <summary>
    /// Supprime les codes relatifs aux ERP (MKGT, GPI...)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="codeType"></param>
    /// <param name="session"></param>
    /// <returns>EtablissementClient</returns>
    Task<TEtablissementClient> DeleteErpCodeAsync(int id, string codeType, ContextSession session);
}
