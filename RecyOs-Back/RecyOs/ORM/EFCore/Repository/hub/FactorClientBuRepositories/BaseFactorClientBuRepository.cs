// <copyright file="BaseFactorClientBuRepository.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Exceptions;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository
{
    public class BaseFactorClientBuRepository<T> : IBaseFactorClientBuRepository<T>
        where T : FactorClientBu, new()
    {
        private readonly DataContext _context;
        private readonly ITokenInfoService _tokenInfoService;
        private readonly DbSet<T> _dbSet;

        public BaseFactorClientBuRepository(
            DataContext context,
            ITokenInfoService tokenInfoService
        )
        {
            _context = context;
            _tokenInfoService = tokenInfoService;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity, ContextSession session)
        {
            try
            {
                var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.IdClient == entity.IdClient && e.IdBu == entity.IdBu);

                if (existingEntity != null)
                {
                    if (existingEntity.IsDeleted)
                    {
                        existingEntity.IsDeleted = false;
                        existingEntity.UpdatedAt = DateTime.Now;
                        existingEntity.UpdatedBy = _tokenInfoService.GetCurrentUserName();

                        // Update the entity explicitly without modifying the primary keys
                        _context.Entry(existingEntity).Property(e => e.IsDeleted).IsModified = true;
                        _context.Entry(existingEntity).Property(e => e.UpdatedAt).IsModified = true;
                        _context.Entry(existingEntity).Property(e => e.UpdatedBy).IsModified = true;
                    
                        await _context.SaveChangesAsync();
                        return existingEntity;
                    }
                    else
                    {
                        throw new InvalidOperationException("Entity with the given keys already exists.");
                    }
                }

                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;    
            }
            catch (DbUpdateException e)
            {
                throw new InvalidOperationException("An error occurred while updating the database.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryException("An unexpected error occurred.", e);
            }
        }
        
        public async Task<IReadOnlyList<T>> GetListAsync(ContextSession session, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }
            return await query.ToListAsync();
        }
        
        public async Task<IReadOnlyList<T>> GetByClientIdAsync(ContextSession session, int clientId, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return await query.Where(e => e.IdClient == clientId).AsNoTracking().ToListAsync();
        }

        
        public async Task<IReadOnlyList<T>> GetByBuIdAsync(ContextSession session, int buId, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }
            
            var existingBu = await query.AnyAsync(b => b.IdBu == buId);
            if (!existingBu)
            {
                throw new InvalidOperationException("Bu with the given Id does not exist in factor.");
            }
            
            return await query.Where(e => e.IdBu == buId).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int clientId, int buId, ContextSession session)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.IdClient == clientId && e.IdBu == buId);
            if (existingEntity == null)
            {
                throw new InvalidOperationException("Entity with the given keys does not exist.");
            }

            // Ensure the primary keys are not altered
            if (_context.Entry(existingEntity).State == EntityState.Detached)
            {
                _dbSet.Attach(existingEntity);
            }

            existingEntity.IsDeleted = true;
            existingEntity.UpdatedAt = DateTime.Now;
            existingEntity.UpdatedBy = _tokenInfoService.GetCurrentUserName();

            // Update the entity explicitly without modifying the primary keys
            _context.Entry(existingEntity).Property(e => e.IsDeleted).IsModified = true;
            _context.Entry(existingEntity).Property(e => e.UpdatedAt).IsModified = true;
            _context.Entry(existingEntity).Property(e => e.UpdatedBy).IsModified = true;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
