// /** UserRoleRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class UserRoleRepository : IUserRoleRepository<UserRole>
{
    private readonly DataContext _dbContext;

    public UserRoleRepository(DataContext context)
    {
        _dbContext = context;
    }

    protected DataContext GetContext(ContextSession session)
    {
        _dbContext.Session = session;
        return _dbContext;
    }

    public async Task<UserRole> Add(UserRole userRole, ContextSession session)
    {
        var context = GetContext(session);
        context.Entry(userRole).State = EntityState.Added;
        await context.SaveChangesAsync();
        await context.Entry(userRole).Reference(ur => ur.Role).LoadAsync();
        return userRole;
    }

    public async Task Delete(int userId, int roleId, ContextSession session)
    {
        var context = GetContext(session);
        var itemToDelete = new UserRole { UserId = userId, RoleId = roleId };
        context.Entry(itemToDelete).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }

    public async Task<UserRole> Get(int userId, int roleId, ContextSession session)
    {
        var context = GetContext(session);
        return await context.Set<UserRole>()
            .AsNoTracking()
            .Where(obj => obj.RoleId == roleId && obj.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<string>> GetByUserId(int userId, ContextSession session)
    {
        var context = GetContext(session);
        return await context.Set<UserRole>()
            .AsNoTracking()
            .Where(obj => obj.UserId == userId)
            .Include(obj => obj.Role)
            .Select(obj => obj.Role.Name)
            .ToListAsync();
    }
}