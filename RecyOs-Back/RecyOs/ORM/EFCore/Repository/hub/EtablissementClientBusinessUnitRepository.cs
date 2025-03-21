using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository;

public class EtablissementClientBusinessUnitRepository : IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit>
{
    private readonly DataContext _context;
    private readonly ITokenInfoService _tokenInfoService;
    public EtablissementClientBusinessUnitRepository(
        DataContext context,
        ITokenInfoService tokenInfoService)
    {
        _context = context;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<EtablissementClientBusinessUnit> CreateAsync(EtablissementClientBusinessUnit etablissementClientBusinessUnit, ContextSession session)
    {
        if (etablissementClientBusinessUnit == null) return null;
        
        var entityExists = await _context.EtablissementClientBusinessUnits
            .Where(ecbu => ecbu.ClientId == etablissementClientBusinessUnit.ClientId && ecbu.BusinessUnitId == etablissementClientBusinessUnit.BusinessUnitId)
            .FirstOrDefaultAsync();

        if (entityExists != null)
        {
            entityExists.IsDeleted = false;
            entityExists.UpdatedAt = DateTime.Now;
            entityExists.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            await _context.SaveChangesAsync();
            
            return entityExists;
        }
        
        _context.Add(etablissementClientBusinessUnit);
        await _context.SaveChangesAsync();
        return etablissementClientBusinessUnit;
    }

    public async Task<IList<BusinessUnit>> GetBusinessUnitsByEtablissementClientIdAsync(int etablissementClientId, ContextSession session, bool includeDeleted = false)
    {
        if (etablissementClientId < 1) return null;
        
        if (!includeDeleted)
        {
            return await _context.EtablissementClientBusinessUnits
                .Where(ecbu => ecbu.ClientId == etablissementClientId && !ecbu.IsDeleted)
                .AsNoTracking()
                .Select(ecbu => ecbu.BusinessUnit)
                .ToListAsync();
        }
        return await _context.EtablissementClientBusinessUnits
            .Where(ecbu => ecbu.ClientId == etablissementClientId)
            .AsNoTracking()
            .Select(ecbu => ecbu.BusinessUnit)
            .ToListAsync();
    }
    
    public async Task<EtablissementClientBusinessUnit> GetByEtablissementClientIdAndBusinessUnitIdAsync(int etablissementClientId, int businessUnitId, ContextSession session, bool includeDeleted = false)
    {
        if (etablissementClientId < 1 || businessUnitId < 1) return null;

        if (!includeDeleted)
        {
            return await _context.EtablissementClientBusinessUnits
                .Where(ecbu => ecbu.ClientId == etablissementClientId && ecbu.BusinessUnitId == businessUnitId && !ecbu.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        
        return await _context.EtablissementClientBusinessUnits
            .Where(ecbu => ecbu.ClientId == etablissementClientId && ecbu.BusinessUnitId == businessUnitId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceResult> UpdateClientIdInBUsAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session)
    {
        try
        {
            // Retrieve the list of EtablissementClientBusinessUnit entities associated with the old client ID
            var buList = await _context.EtablissementClientBusinessUnits
                .Where(ecbu => ecbu.ClientId == oldEtablissementClientId)
                .ToListAsync();

            // Mark the old entities as deleted
            foreach (var ecbu in buList)
            {
                await DeleteAsync(ecbu.ClientId, ecbu.BusinessUnitId, session);
                ecbu.UpdatedAt = DateTime.Now;
                ecbu.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            }

            // Create and add new entities
            foreach (var ecbu in buList)
            {
                var newEtablissementClientBusinessUnit = new EtablissementClientBusinessUnit
                {
                    ClientId = newEtablissementId,
                    BusinessUnitId = ecbu.BusinessUnitId,
                    CreatedBy = ecbu.CreatedBy,
                    CreateDate = ecbu.CreateDate,
                    UpdatedBy = _tokenInfoService.GetCurrentUserName(),
                    UpdatedAt = DateTime.Now
                };
                await CreateAsync(newEtablissementClientBusinessUnit, session);
            }

           await _context.SaveChangesAsync();

            var verificationList = await GetBusinessUnitsByEtablissementClientIdAsync(newEtablissementId, session);

            if (verificationList.Count != buList.Count)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Une erreur est survenue lors du transfert des Business Units"
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
    
    public async Task<bool> DeleteAsync(int etablissementClientId, int businessUnitId, ContextSession session)
    {
        if (etablissementClientId < 1 || businessUnitId < 1) return false;

        var ecbu = await _context.EtablissementClientBusinessUnits
            .Where(ecbu => ecbu.ClientId == etablissementClientId && ecbu.BusinessUnitId == businessUnitId && !ecbu.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (ecbu == null) return false;

        ecbu.IsDeleted = true;
        ecbu.UpdatedAt = DateTime.Now;
        ecbu.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        await _context.SaveChangesAsync();
        
        ecbu = await GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, session);
        if (ecbu != null) return false;
        
        return true;
    }
}