// /** FuseNavigationMenuRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/05/2023
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Fuse Framework
//  */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.Extensions;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class FuseNavigationMenuRepository: BaseDeletableRepository<FuseNavigationItem, DataContext>, IFuseNavigationMenuRepository<FuseNavigationItem>
{
    public FuseNavigationMenuRepository(DataContext context) : base(context)
    {
    }
    
    public async Task<(IEnumerable<FuseNavigationItem>, int)> GetFiltredListWithCount(FuseNavigationMenuFilter filter, ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted)
            .ApplyFilter(filter);
        return (
            await query
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToArrayAsync(),
            await query.CountAsync());
    }
    
    public async Task<FuseNavigationItem> GetByName(string name, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Title == name)
            .FirstOrDefaultAsync();
    }
    
    public async Task<FuseNavigationItem> Update(FuseNavigationItem nature, ContextSession session)
    {
        var objectExists = await Exists(nature, session);
        var context = GetContext(session);
        context.Entry(nature).State = objectExists ? EntityState.Modified : EntityState.Added;

        if (string.IsNullOrEmpty(nature.Title))
        {
            context.Entry(nature).Property(x => x.Title).IsModified = false;
        }

        await context.SaveChangesAsync();
        return nature;
    }
    
    public async Task<IEnumerable<FuseNavigationItem>> GetMenu(ContextSession session, List<int> roleIds, bool includeDeleted = false, int menuId = 1)
    {
        // Get top level items
        var topItems = await GetEntities(session, includeDeleted)
            .Include(c => c.Role)
            .Include(c => c.Badge)
            .Where(obj => obj.MenuId == menuId && roleIds.Contains(obj.RoleId))
            .OrderBy(c => c.MenuPosition)
            .ToListAsync();

        // Get all children that match the roleIds
        var children = await GetEntities(session, includeDeleted)
            .Include(c => c.Role)
            .Include(c => c.Badge)
            .Where(obj => roleIds.Contains(obj.RoleId) && obj.ParentId.HasValue)
            .ToListAsync();

        // Manually associate children with their parents
        foreach (var item in topItems)
        {
            item.Children = children.Where(c => c.ParentId == item.Id).ToList();
        }

        return topItems;
    }
}