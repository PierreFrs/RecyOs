//  FuseNavigationMenuController.cs -
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
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("fuse-navigation-menu")]
public class FuseNavigationMenuController: BaseApiController
{
    protected readonly IFuseNavigationMenuService _fuseNavigationMenuService;
    
    public FuseNavigationMenuController(IFuseNavigationMenuService fuseNavigationMenuService)
    {
        _fuseNavigationMenuService = fuseNavigationMenuService;
    }
    
    /// <summary>
    /// Permet d'obtenir un menu par son id
    /// </summary>
    /// <param name="id">Identifiant de l'iobjet</param>
    /// <returns>Objet FuseNavigationItemDTO</returns>
    [SwaggerResponse(200, "Objet FuseNavigationMenu", typeof(FuseNavigationItemDto))]
    [SwaggerResponse(404, "Objet FuseNavigationMenu non trouvé")]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> Get(int id)
    {
        FuseNavigationItemDto fuseNavigationMenu = await _fuseNavigationMenuService.GetById(id);
        if (fuseNavigationMenu == null) return NotFound();
        return Ok(fuseNavigationMenu);
    }
    
    /// <summary>
    /// Permet d'obtenir la liste des menus
    /// </summary>
    /// <returns>Liste de FuseNavigationItemDTO</returns>
    [SwaggerResponse(200, "Liste de FuseNavigationMenu", typeof(List<FuseNavigationItemDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] FuseNavigationMenuFilter filter)
    {
        filter = filter ?? new FuseNavigationMenuFilter();
        var fuseNavigationMenu = await _fuseNavigationMenuService.GetDataForGrid(filter);
        return Ok(fuseNavigationMenu);
    }
    
    /// <summary>
    /// Permet de créer un menu
    /// </summary>
    /// <param name="fuseNavigationMenuDto">Objet à créer</param>
    /// <returns>Objet FuseNavigationItemDTO crée</returns>
    [SwaggerResponse(200, "Objet FuseNavigationMenu crée", typeof(FuseNavigationItemDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] FuseNavigationItemDto fuseNavigationMenuDto)
    {
        FuseNavigationItemDto fuseNavigationMenu = await _fuseNavigationMenuService.Edit(fuseNavigationMenuDto);
        return Ok(fuseNavigationMenu);
    }
    
    /// <summary>
    /// Permet de modifier un menu
    /// </summary>
    /// <param name="id">id de l'objet à modifier</param>
    /// <param name="fuseNavigationMenuDto">Objet à modifier</param>
    /// <returns>Objet FuseNavigationItemDTO modifié</returns>
    [SwaggerResponse(200, "Objet FuseNavigationMenu modifié", typeof(FuseNavigationItemDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpPut]
    [Route("")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] FuseNavigationItemDto fuseNavigationMenuDto)
    {
        FuseNavigationItemDto fuseNavigationMenu = await _fuseNavigationMenuService.Edit(fuseNavigationMenuDto);
        return Ok(fuseNavigationMenu);
    }
    
    /// <summary>
    /// Permet de supprimer un menu
    /// </summary>
    /// <param name="id">id de l'objet à supprimer</param>
    /// <returns>Objet FuseNavigationItemDTO supprimé</returns>
    [SwaggerResponse(200, "Objet FuseNavigationMenu supprimé", typeof(FuseNavigationItemDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        FuseNavigationItemDto fuseNavigationMenu = await _fuseNavigationMenuService.GetById(id);
        if (fuseNavigationMenu == null) return NotFound();
        await _fuseNavigationMenuService.Delete(id);
        return Ok(fuseNavigationMenu);
    }
    
    /// <summary>
    /// Permet de récupérer le menu par son ID
    /// </summary>
    /// <param name="menuId">id du menu</param>
    /// <returns>Liste de FuseNavigationItemDTO</returns>
    [SwaggerResponse(200, "Liste de FuseNavigationMenu", typeof(List<NavigationDto>))]
    [SwaggerResponse(401, "Non autorisé")]
    [HttpGet]
    [Route("get-menu")]
    [Authorize(Policy = "All")]
    public async Task<IActionResult> GetMenu([FromQuery] int menuId)
    {
        NavigationDto valRetour = new NavigationDto();
        IEnumerable<FuseNavigationItemDto> fuseNavigationMenu = await _fuseNavigationMenuService.GetMenu(menuId: menuId);
        if (fuseNavigationMenu == null) return NotFound();
        valRetour.Compact = fuseNavigationMenu;
        valRetour.Default = fuseNavigationMenu;
        valRetour.Futuristic = fuseNavigationMenu;
        valRetour.Horizontal = fuseNavigationMenu;
        return Ok(valRetour);
    }
}