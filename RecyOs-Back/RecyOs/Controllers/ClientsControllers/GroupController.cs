using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Entities;
using Swashbuckle.AspNetCore.Annotations;
namespace RecyOs.Controllers.ClientsControllers;

/// <summary>
/// Controller for managing client groups
/// Provides endpoints for CRUD operations on groups that can contain both EtablissementClients and ClientEuropes
/// </summary>

[Route("group")]
[ApiController]
public class GroupController : BaseApiController
{
    private readonly IGroupService _groupService;

    /// <summary>
    /// Initializes a new instance of the GroupController
    /// </summary>
    /// <param name="groupService">The service handling group operations</param>
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    /// <summary>
    /// Retrieves all client groups
    /// </summary>
    /// <returns>A list of all active client groups</returns>
    /// <remarks>
    /// Sample request:
    /// GET /api/group
    /// </remarks>
    [SwaggerResponse(200, "Liste des groupes trouvée", typeof(IReadOnlyList<GroupDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GroupDto>>> GetAll()
    {
        return Ok(await _groupService.GetListAsync());
    }

    /// <summary>
    /// Retrieves a specific client group by its identifier
    /// </summary>
    /// <param name="id">The unique identifier of the group</param>
    /// <returns>The requested group if found</returns>
    /// <remarks>
    /// Sample request:
    /// GET /api/group/1
    /// </remarks>
    [SwaggerResponse(200, "Groupe trouvé", typeof(GroupDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Groupe non trouvé")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetById(int id)
    {
        return Ok(await _groupService.GetByIdAsync(id));
    }

    /// <summary>
    /// Retrieves a specific client group by its name
    /// </summary>
    /// <param name="name">The name of the group</param>
    /// <returns>The requested group if found</returns>
    /// <remarks>
    /// Sample request:
    /// GET /api/group/name/Group1
    /// </remarks>
    [SwaggerResponse(200, "Groupe trouvé", typeof(GroupDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Groupe non trouvé")]
    [HttpGet("name/{name}")]
    public async Task<ActionResult<GroupDto>> GetByName(string name)
    {
        return Ok(await _groupService.GetByNameAsync(name));
    }

    /// <summary>
    /// Creates a new client group
    /// </summary>
    /// <param name="groupDto">The group information to create</param>
    /// <returns>The newly created group</returns>
    /// <remarks>
    /// Sample request:
    /// POST /api/group
    /// {
    ///     "name": "New Group",
    ///     "description": "Group description"
    /// }
    /// </remarks>    
    [SwaggerResponse(200, "Groupe créé", typeof(GroupDto))]
    [SwaggerResponse(400, "Données de groupe invalides")]
    [SwaggerResponse(401, "Non autorisé")]
    [Authorize(Policy = "CreateGroup")]
    [HttpPost]
    public async Task<ActionResult<GroupDto>> Create([FromForm] GroupDto groupDto)
    {
        return Ok(await _groupService.CreateAsync(groupDto));
    }

    /// <summary>
    /// Updates an existing client group
    /// </summary>
    /// <param name="id">The unique identifier of the group to update</param>
    /// <param name="groupDto">The updated group information</param>
    /// <returns>The updated group</returns>
    /// <remarks>
    /// Sample request:
    /// PUT /api/group/1
    /// {
    ///     "name": "Updated Group Name",
    ///     "description": "Updated description"
    /// }
    /// </remarks>
    /// <response code="200">Returns the updated group</response>
    /// <response code="400">If the group data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the group is not found</response>
    [SwaggerResponse(200, "Groupe mis à jour", typeof(GroupDto))]
    [SwaggerResponse(400, "Données de groupe invalides")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Groupe non trouvé")]
    [Authorize(Policy = "UpdateGroup")]
    [HttpPut("{id}")]
    public async Task<ActionResult<GroupDto>> Update(int id, [FromForm] GroupDto groupDto)
    {
        return Ok(await _groupService.UpdateAsync(id, groupDto));
    }

    /// <summary>
    /// Deletes a specific client group
    /// </summary>
    /// <param name="id">The unique identifier of the group to delete</param>
    /// <returns>True if the deletion was successful</returns>
    /// <remarks>
    /// Sample request:
    /// DELETE /api/group/1
    /// </remarks>
    [SwaggerResponse(200, "Suppression réussie", typeof(bool))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Groupe non trouvé")]
    [HttpDelete("{id}")]
    [Authorize(Policy = "DeleteGroup")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        return Ok(await _groupService.DeleteAsync(id));
    }

    /// <summary>
    /// Retrieves a filtered list of client groups with associated clients
    /// </summary>
    /// <param name="filter">The filter criteria for the groups</param>
    /// <returns>A list of groups with their associated clients</returns>
    /// <remarks>
    /// Sample request:
    /// GET /api/group/filtered?FilteredByNom=Group1
    /// </remarks>
    [SwaggerResponse(200, "Liste filtrée des groupes", typeof(IReadOnlyList<GroupDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(400, "Critères de filtrage invalides")]
    [HttpGet("filtered")]
    public async Task<ActionResult<IReadOnlyList<GroupDto>>> GetFilteredListWithClients([FromQuery] GroupFilter filter)
    {
        filter = filter ?? new GroupFilter();
        return Ok(await _groupService.GetFilteredListWithClientsAsync(filter));
    }
} 