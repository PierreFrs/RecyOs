//  EtablissementFicheController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;
[Route("etablissement_fiche")]
public class EtablissementFicheController : BaseApiController
{
    protected readonly IEtablissementFicheService _etablissemntFicheService;
    
    public EtablissementFicheController(IEtablissementFicheService etablissemntFicheService)
    {
        _etablissemntFicheService = etablissemntFicheService;
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementFiche à partir de son identifiant
    /// </summary>
    /// <Param name="id">Identifiant de l'etablissementFiche</Param>
    /// <returns>Objet EtablissementFiche</returns>
    [SwaggerResponse(200, "EtablissementFiche trouvé", typeof(EtablissementFicheDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementFiche non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var etablissementFiche = await _etablissemntFicheService.GetById(id);
        if (etablissementFiche == null) return NotFound();
        return Ok(etablissementFiche);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des objets EtablissementFiche
    /// </summary>
    /// <param name="filter">Filtre de recherche</param>
    /// <returns>Liste des objets EtablissementFiche</returns>
    [SwaggerResponse(200, "Liste des objets EtablissementFiche", typeof(List<EtablissementFicheDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] EtablissementFicheGridFilter filter)
    {
        filter = filter ?? new EtablissementFicheGridFilter();
        var etablissementFiche = await _etablissemntFicheService.GetDataForGrid(filter);
        return Ok(etablissementFiche);
    }
    
    /// <summary>
    /// Permet d'obtenir un objet EtablissementFiche à partir de son siret
    /// </summary>
    /// <param name="siret">Siret de l'etablissementFiche</param>
    /// <returns>Objet EtablissementFiche</returns>
    [SwaggerResponse(200, "EtablissementFiche trouvé", typeof(EtablissementFicheDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementFiche non trouvé")]
    [HttpGet]
    [Route("siret/{siret}")]
    public async Task<IActionResult> GetBySiret(string siret)
    {
        var etablissementFiche = await _etablissemntFicheService.GetBySiret(siret);
        if (etablissementFiche == null) return NotFound();
        return Ok(etablissementFiche);
    }
    
    /// <summary>
    /// Permet de créer un objet EtablissementFiche
    /// </summary>
    /// <param name="etablissementFicheDto">Objet à créer</param>
    /// <returns>Objet crée</returns>
    [SwaggerResponse(200, "EtablissementFiche crée", typeof(EtablissementFicheDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] EtablissementFicheDto etablissementFicheDto)
    {
        var etablissementFiche = await _etablissemntFicheService.Edit(etablissementFicheDto);
        return Ok(etablissementFiche);
    }
    
    /// <summary>
    /// Permet de modifier un objet EtablissementFiche
    /// </summary>
    /// <param name="etablissementFicheDto">Objet à modifier</param>
    /// <returns>Objet modifié</returns>
    [SwaggerResponse(200, "EtablissementFiche modifié", typeof(EtablissementFicheDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Edit([FromBody] EtablissementFicheDto etablissementFicheDto)
    {
        return await Create(etablissementFicheDto);
    }
    
    /// <summary>
    /// Permet de supprimer un objet EtablissementFiche
    /// </summary>
    /// <param name="id">Identifiant de l'objet à supprimer</param>
    /// <returns>Objet supprimé</returns>
    [SwaggerResponse(200, "EtablissementFiche supprimé", typeof(EtablissementFicheDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var etablissementFiche = await _etablissemntFicheService.GetById(id);
        if (etablissementFiche == null) return NotFound();
        var deleted = await _etablissemntFicheService.Delete(id);
        return Ok(deleted);
    }
    
    
}