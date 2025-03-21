//  UsersRolesController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("users/{id:int}/roles")]
[Authorize(Policy = "SuperAdmin")]
public class UsersRolesController : BaseApiController
{
    protected readonly IUserRoleService UserRoleService;

    public UsersRolesController(IUserRoleService userRoleService)
    {
        this.UserRoleService = userRoleService;
    }

    /// <summary>
    /// Permet d'ajouter un role à un utlisateur
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur</param>
    /// <param name="role">Role à ajouter</param>
    /// <returns></returns>
    [SwaggerResponse(200,"OK le role à bien été ajouté")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> Assign(int id, RoleDto role)
    {
        var result = await UserRoleService.AssignToRole(id, role.Name);
        if (result.Succeeded)
            return Ok();

        return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description });
    }

    /// <summary>
    /// Permet  de supprimer un role à un utilisateur 
    /// </summary>
    /// <param name="id">Identifiant de l'utlisateur</param>
    /// <param name="role">Role à supprimer</param>
    /// <returns></returns>
    [SwaggerResponse(200, "Ok le role à bien été supprimé")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> Unassign(int id, RoleDto role)
    {
        var result = await UserRoleService.UnassignRole(id, role.Name);
        if (result.Succeeded)
            return Ok();

        return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description });
    }

    /// <summary>
    /// Permet de lister les roles d'un utilisateur
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur</param>
    /// <returns></returns>
    [SwaggerResponse(200,"Liste de roles", typeof(List<string>))]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetRoles(int id)
    {
        var result = await UserRoleService.GetRoles(id);
        return Ok(result);
    }
}