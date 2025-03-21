// /** RoleRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class RoleRepository: BaseRepository<Role, DataContext>, IRoleRepository<Role>
{
    public RoleRepository(DataContext context) : base(context)
    {
    }

    public async Task<Role> Get(string name, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Name == name)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Role> Get(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IList<Role>> GetList(ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .ToListAsync();
    }

    public async Task<IList<Role>> GetListByUserId(int userId, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session)
            .Where(obj => obj.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync();
    }
}