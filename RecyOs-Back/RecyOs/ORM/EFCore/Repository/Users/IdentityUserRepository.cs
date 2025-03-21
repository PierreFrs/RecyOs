// /** IdentityUserRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 24/01/2021
//  * Fichier Modifié le : 24/01/2021
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

public class IdentityUserRepository: BaseDeletableRepository<User, DataContext>, IIdentityUserRepository<User>
    {
        public IdentityUserRepository(DataContext context) : base(context)
        {
        }

        public override async Task<User> Get(int id, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Where(obj => obj.Id == id)
                .Include(u => u.Claims)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByLogin(string login, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Where(obj => obj.UserName == login)
                .Include(u => u.Claims)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .Include(u => u.Claims)
                .Where(obj => obj.Email == email)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetById(int id, ContextSession session, bool includeDeleted = false)
        {
            return Get(id, session);
        }

        public async Task<IList<User>> GetUsersByRole(int roleId, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Include(u => u.Claims)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x => x.UserRoles.Any(ur => ur.RoleId == roleId))
                .ToArrayAsync();
        }

        public async Task<IList<User>> GetUsersByClaim(string claimType, string claimValue, ContextSession session,
            bool includeDeleted = false)
        {
            return await GetEntities(session)
                .Include(u => u.Claims)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x => x.Claims.Any(cl => cl.ClaimType == claimType && cl.ClaimValue == claimValue))
                .ToArrayAsync();
        }
    }