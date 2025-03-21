// /** BaseDeletableRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 07/01/2023
//  * Code développé pour le projet : Archimede.Common
//  * Code adapté pour le projet : RecyOs server
//  */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.EFCore.Repository;

public abstract class BaseDeletableRepository<TType, TContext>
        where TType : DeletableEntity, new()
        where TContext : DataContext
    {
        private readonly TContext _dbContext;

        protected BaseDeletableRepository(TContext context)
        {
            _dbContext = context;
        }

        protected TContext GetContext(ContextSession session)
        {
            _dbContext.Session = session;
            return _dbContext;
        }

        protected IQueryable<TType> GetEntities(ContextSession session, bool includeDeleted = false)
        {
            var query = GetContext(session).Set<TType>().AsQueryable();
            if (!includeDeleted)
            {
                return query.Where(obj => !obj.IsDeleted)
                    .AsNoTracking();
            }

            return query.Where(obj => obj.Id > 0)
                .AsNoTracking();
        }

        public virtual async Task<IEnumerable<TType>> GetList(ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted).AsNoTracking().ToListAsync();
        }

        public virtual async Task<TType> Get(int id, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted)
                .AsNoTracking()
                .Where(obj => obj.Id == id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<bool> Exists(TType obj, ContextSession session, bool includeDeleted = false)
        {
            return await GetEntities(session, includeDeleted)
                .Where(x => x.Id == obj.Id)
                .CountAsync() > 0;
        }

        public virtual async Task<TType> Edit(TType obj, ContextSession session)
        {
            var objectExists = await Exists(obj, session);
            _dbContext.Entry(obj).State = objectExists ? EntityState.Modified : EntityState.Added;
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public virtual async Task Delete(int id, ContextSession session)
        {
            // Trouvez l'entité existante et détachez-la si elle est suivie
            var existingEntity = await _dbContext.Set<TType>().FindAsync(id);
            if (existingEntity != null)
            {
                existingEntity.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }