// /** IFuseNavigationMenuService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/05/2023
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Fuse Framework
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface IFuseNavigationMenuService
{
    Task<GridData<FuseNavigationItemDto>> GetDataForGrid(FuseNavigationMenuFilter filter, bool includeDeleted = false);
    Task<FuseNavigationItemDto> GetById(int id, bool includeDeleted = false);
    Task<FuseNavigationItemDto> GetByNom(string nom, bool includeDeleted = false);
    Task<bool> Delete(int id);
    Task<FuseNavigationItemDto> Edit(FuseNavigationItemDto dto);
    Task<IEnumerable<FuseNavigationItemDto>> GetMenu(bool includeDeleted = false, int menuId = 1);
}