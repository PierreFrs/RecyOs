//  RolesController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("roles")]
public class RolesController: BaseApiController
{
    protected readonly IRoleService UserRoleService;
    
    public RolesController(IRoleService userRoleService)
    {
        this.UserRoleService = userRoleService;
    }
    
    /// <summary>
    /// Permet d'obtenir un objet Role à partir de son identifiant
    /// </summary>
    /// <param name="id">Identifiant du role</param>
    /// <returns>Objet Role</returns>
    [SwaggerResponse(200, "Role trouvé", typeof(RoleDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Role non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var role = await UserRoleService.GetRoleById(id);
        if (role == null) return NotFound();
        return Ok(role);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des objets Role
    /// </summary>
    /// <returns>Liste des objets Role </returns>
    [SwaggerResponse(200, "Liste des objets Role", typeof(List<RoleDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var roles = await UserRoleService.GetRoles();
        return Ok(roles);
    }
    
}