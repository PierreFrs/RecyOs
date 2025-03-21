// BalanceRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 29/08/2024
// Fichier Modifié le : 29/08/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class BalanceRepository<TEntity> : IBalanceRepository<TEntity>
    where TEntity : Balance, IClientBalance, new()
{
    private readonly DataContext _context;
    private readonly ITokenInfoService _tokenInfoService;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public BalanceRepository(
        DataContext context,
        ITokenInfoService tokenInfoService
        )
    {
        _context = context;
        _tokenInfoService = tokenInfoService;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity, ContextSession session)
    {
        try
        {
            var entityExists = await _dbSet
                .Where(q => q.ClientId == entity.ClientId && q.SocieteId == entity.SocieteId)
                .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                entityExists.UpdatedBy = _tokenInfoService.GetCurrentUserName();
                entityExists.UpdatedAt = DateTime.Now;
                entityExists.Montant = entity.Montant;
                entityExists.IsDeleted = false;
                await _context.SaveChangesAsync();
                return entityExists;
            }

            entity.CreatedBy = _tokenInfoService.GetCurrentUserName();
            entity.CreateDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw; // Let the exception propagate!
        }
    }
    
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(ContextSession session, bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return await _dbSet
                .ToListAsync();    
        }
        
        return await _dbSet
            .Where(q => !q.IsDeleted)
            .ToListAsync();
    }
    
    public async Task<TEntity> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        try
        {
            if (includeDeleted)
            {
                return await _dbSet
                    .Where(q => q.Id == id)
                    .FirstOrDefaultAsync();
            }
            return await _dbSet
                .Where(q => q.Id == id && !q.IsDeleted)
                .FirstOrDefaultAsync();
        }
        catch (EntityNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task<IReadOnlyList<TEntity>> GetByClientIdAsync(int clientId, ContextSession session, bool includeDeleted = false)
    {
        try
        {
            if (includeDeleted)
            {
                return await _dbSet
                    .Where(q => q.ClientId == clientId)
                    .ToListAsync();
            }
            return await _dbSet
                .Where(q => q.ClientId == clientId && !q.IsDeleted)
                .ToListAsync();
        }
        catch (EntityNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<ServiceResult> UpdateClientIdInBalancesAsync(int oldClientId, int newId, ContextSession session)
    {
        try
        {
            // Retrieve the list of Balance entities associated with the old client ID
            var balanceList = await _dbSet
                .Where(q => q.ClientId == oldClientId)
                .ToListAsync();
    
            // Mark old entities as deleted
            foreach (var balance in balanceList)
            {
                balance.IsDeleted = true;
                balance.UpdatedAt = DateTime.Now;
                balance.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            }
            
            // Create and add new entities
            foreach (var balance in balanceList)
            {
                var newEntity = new TEntity
                {
                    ClientId = newId,
                    SocieteId = balance.SocieteId,
                    Montant = balance.Montant,
                    CreatedBy = balance.CreatedBy,
                    CreateDate = balance.CreateDate,
                    UpdatedBy = balance.UpdatedBy,
                    UpdatedAt = balance.UpdatedAt
                };
                await _dbSet.AddAsync(newEntity);
            }
            
            await _context.SaveChangesAsync();
            
            // Verify the changes
            var verificationList = await _dbSet
                .Where(q => q.ClientId == newId)
                .AsNoTracking()
                .ToListAsync();
            
            if (verificationList.Count != balanceList.Count)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Une erreur est survenue lors du transfert des balances"
                };
            }
            else
            {
                return new ServiceResult
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Les ids ont été mis à jour"
                };
            }
        }
        catch (Exception e)
        {
            return new ServiceResult
            {
                Success = false,
                StatusCode = 500,
                Message = e.Message
            };
        }
    }
    
    public async Task<TEntity> UpdateAsync(int id, TEntity entity, ContextSession session)
    {
        try
        {
            var entityToUpdate = await _dbSet
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync();
            
            entityToUpdate.ClientId = entity.ClientId;
            entityToUpdate.SocieteId = entity.SocieteId;
            entityToUpdate.Montant = entity.Montant;
            entityToUpdate.UpdatedAt = DateTime.Now;
            entityToUpdate.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            
            await _context.SaveChangesAsync();
            return entityToUpdate;
        }
        catch (EntityNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        try
        {
            var entity = await _dbSet
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync();
        
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        
            await _context.SaveChangesAsync();
            return true;
        }
        catch (EntityNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}