using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.IParameters;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("parameter")]
public class ParameterController : BaseApiController
{
    private readonly IParameterService _parameterService;

    public ParameterController(IParameterService parameterService)
    {
        _parameterService = parameterService;
    }

    /// <summary>
    /// Récupère un paramètre par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du paramètre.</param>
    /// <returns>Retourne le paramètre correspondant à l'identifiant donné.</returns>
    [SwaggerResponse(200, "Paramètre trouvé", typeof(ParameterDto))]
    [SwaggerResponse(404, "Paramètre non trouvé")]
    [HttpGet("{id:int}")]
    [Authorize(Policy = "ReadParameter")]
    public async Task<IActionResult> Get(int id)
    {
        var parameter = await _parameterService.GetById(id);
        if (parameter == null)
        {
            return NotFound();
        }
        return Ok(parameter);
    }

    /// <summary>
    /// Récupère les données filtrées pour une grille de paramètres.
    /// </summary>
    /// <param name="filter">Les critères de filtrage pour la récupération des données.</param>
    /// <returns>Retourne une liste de paramètres filtrés.</returns>
    [SwaggerResponse(200, "Liste des paramètres", typeof(IEnumerable<ParameterDto>))]
    [HttpGet]
    [Authorize(Policy = "ReadParameter")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] ParameterFilter filter)
    {
        var gridData = await _parameterService.GetDataForGrid(filter);
        return Ok(gridData);
    }

    /// <summary>
    /// Crée un nouveau paramètre.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations du paramètre à créer.</param>
    /// <returns>Retourne le paramètre créé.</returns>
    [SwaggerResponse(200, "Paramètre créé", typeof(ParameterDto))]
    [SwaggerResponse(400, "Paramètre non créé")]
    [HttpPost]
    [Authorize(Policy = "CreateParameter")]
    public async Task<IActionResult> Create([FromBody] ParameterDto dto)
    {
        if (dto == null)
        {
            return BadRequest();
        }

        var createdParameter = await _parameterService.CreateAsync(dto);
        return Ok(createdParameter);
    }

    /// <summary>
    /// Met à jour un paramètre existant.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations mises à jour du paramètre.</param>
    /// <returns>Retourne le paramètre mis à jour.</returns>
    [SwaggerResponse(200, "Paramètre mis à jour", typeof(ParameterDto))]
    [SwaggerResponse(404, "Paramètre non trouvé")]
    [HttpPut]
    [Authorize(Policy = "UpdateParameter")]
    public async Task<IActionResult> Update([FromBody] ParameterDto dto)
    {
        if (dto == null)
        {
            return BadRequest();
        }

        var updatedParameter = await _parameterService.UpdateAsync(dto);
        if (updatedParameter == null)
        {
            return NotFound();
        }

        return Ok(updatedParameter);
    }

    /// <summary>
    /// Supprime un paramètre par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du paramètre à supprimer.</param>
    /// <returns>Retourne un booléen indiquant si la suppression a réussi.</returns>
    [SwaggerResponse(200, "Paramètre supprimé")]
    [SwaggerResponse(404, "Paramètre non trouvé")]
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "DeleteParameter")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _parameterService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Récupère tous les modules disponibles.
    /// </summary>
    /// <returns>Retourne une liste de modules.</returns>
    [SwaggerResponse(200, "Liste des modules", typeof(IEnumerable<string>))]
    [HttpGet("modules")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> GetAllModules()
    {
        var modules = await _parameterService.GetAllModulesAsync();
        return Ok(modules);
    }

    /// <summary>
    /// Récupère tous les paramètres d'un module.
    /// </summary>
    /// <param name="module">Le module à récupérer.</param>
    /// <returns>Retourne une liste de paramètres.</returns>
    [SwaggerResponse(200, "Liste des paramètres", typeof(IEnumerable<ParameterDto>))]
    [HttpGet("module/{module}")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> GetAllByModule(string module)
    {
        var parameters = await _parameterService.GetAllByModuleAsync(module);
        return Ok(parameters);
    }

    /// <summary>
    /// Récupère un paramètre par son module et son nom.
    /// </summary>
    /// <param name="module">Le module du paramètre.</param>
    /// <param name="nom">Le nom du paramètre.</param>
    /// <returns>Retourne le paramètre correspondant au module et au nom donnés.</returns>
    [SwaggerResponse(200, "Paramètre trouvé", typeof(ParameterDto))]
    [SwaggerResponse(404, "Paramètre non trouvé")]
    [HttpGet("module/{module}/nom/{nom}")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> GetByNom(string module, string nom)
    {
        var parameter = await _parameterService.GetByNomAsync(module, nom); 
        if (parameter == null)
        {
            return NotFound();
        }
        return Ok(parameter);
    }
}
