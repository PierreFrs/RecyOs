// /** IFuseNavigationMenuRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/05/2023
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Fuse Framework
//  */
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface IFuseNavigationMenuRepository<TFuseNavigationMenu> where TFuseNavigationMenu : FuseNavigationItem
{
    Task Delete(int id, ContextSession session);
    Task<(IEnumerable<TFuseNavigationMenu>,int)> GetFiltredListWithCount(FuseNavigationMenuFilter filter, ContextSession session, bool includeDeleted = false);
    
    Task<TFuseNavigationMenu> GetByName(string name, ContextSession session, bool includeDeleted = false);

    Task<TFuseNavigationMenu> Get(int id, ContextSession session, bool includeDeleted = false);

    Task<TFuseNavigationMenu> Update(TFuseNavigationMenu nature, ContextSession session);
    
    Task<IEnumerable<TFuseNavigationMenu>> GetList(ContextSession session, bool includeDeleted = false);

    Task<IEnumerable<FuseNavigationItem>> GetMenu(ContextSession session, List<int> roleIds,
        bool includeDeleted = false, int menuId = 1);
}