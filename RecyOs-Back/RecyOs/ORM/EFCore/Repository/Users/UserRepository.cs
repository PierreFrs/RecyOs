// /** USerRepository.cs -
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
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.Extensions;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

    public class UserRepository: BaseDeletableRepository<User, DataContext>, IUserRepository<User>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
        
        public override async Task<User> Edit(User obj, ContextSession session)
        {
            var objectExists = await Exists(obj, session);
            var context = GetContext(session);
            context.Entry(obj).State = objectExists ? EntityState.Modified : EntityState.Added;

            if (string.IsNullOrEmpty(obj.Password))
            {
                context.Entry(obj).Property(x => x.Password).IsModified = false;
            }

            await context.SaveChangesAsync();
            return obj;
        }

        public override async Task<User> Get(int id, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted)
                .Where(obj => obj.Id == id)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByLogin(string login, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted)
                .Where(obj => obj.UserName == login)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted)
                .Where(obj => obj.Email == email)
                .Include(u => u.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();
        }

  
        ///     Get all users with pagination
        public async Task<(IEnumerable<User>, int)> GetFilteredListWithTotalCount(UsersGridFilter filter,
            ContextSession session, bool includeDeleted = false)
        {
            var query = GetEntities(session, includeDeleted).ApplyFilter(filter);
            return (
                await query
                    .Skip(filter.PageSize * (filter.PageNumber))
                    .Take(filter.PageSize)
                    .Include(u => u.UserRoles)
                    .ThenInclude(x => x.Role)
                    .ToArrayAsync(),
                await query
                    .CountAsync());
        }
    }