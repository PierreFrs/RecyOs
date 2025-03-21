using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class ClientEuropeBusinessUnitRepository : IClientEuropeBusinessUnitRepository<ClientEuropeBusinessUnit, BusinessUnit>
{
    private readonly DataContext _context;
    private readonly ITokenInfoService _tokenInfoService;
    public ClientEuropeBusinessUnitRepository(
        DataContext context,
        ITokenInfoService tokenInfoService
        )
    {
        _context = context;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<ClientEuropeBusinessUnit> CreateAsync(ClientEuropeBusinessUnit clientEuropeBusinessUnit, ContextSession session)
    {
        if (clientEuropeBusinessUnit == null) return null;
        
        var entityExists = await _context.ClientEuropeBusinessUnits
            .Where(cebu => cebu.ClientId == clientEuropeBusinessUnit.ClientId && cebu.BusinessUnitId == clientEuropeBusinessUnit.BusinessUnitId)
            .FirstOrDefaultAsync();

        if (entityExists != null)
        {
            entityExists.IsDeleted = false;
            entityExists.UpdatedAt = DateTime.Now;
            entityExists.UpdatedBy = _tokenInfoService.GetCurrentUserName();
            await _context.SaveChangesAsync();
            return entityExists;
        }
        
        _context.Add(clientEuropeBusinessUnit);
        await _context.SaveChangesAsync();
        return clientEuropeBusinessUnit;
    }

    public async Task<IList<BusinessUnit>> GetBusinessUnitsByClientEuropeIdAsync(int clientEuropeId, ContextSession session, bool includeDeleted = false)
    {
        if (clientEuropeId < 1) return null;
        
        if (!includeDeleted)
        {
            return await _context.ClientEuropeBusinessUnits
                .Where(cebu => cebu.ClientId == clientEuropeId && !cebu.IsDeleted)
                .AsNoTracking()
                .Select(cebu => cebu.BusinessUnit)
                .ToListAsync();
        }
        
        return await _context.ClientEuropeBusinessUnits
            .Where(cebu => cebu.ClientId == clientEuropeId)
            .AsNoTracking()
            .Select(cebu => cebu.BusinessUnit)
            .ToListAsync();

        
    }
    
    public async Task<ClientEuropeBusinessUnit> GetByClientEuropeIdAndBusinessUnitIdAsync(int clientEuropeId, int businessUnitId, ContextSession session, bool includeDeleted = false)
    {
        if (clientEuropeId < 1 || businessUnitId < 1) return null;

        var clientEuropeBusinessUnit = new ClientEuropeBusinessUnit();

        if (!includeDeleted)
        {
            clientEuropeBusinessUnit = await _context.ClientEuropeBusinessUnits
                .Where(cebu => cebu.ClientId == clientEuropeId && cebu.BusinessUnitId == businessUnitId && !cebu.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            
            return clientEuropeBusinessUnit;
        }
            await _context.ClientEuropeBusinessUnits
            .Where(cebu => cebu.ClientId == clientEuropeId && cebu.BusinessUnitId == businessUnitId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return clientEuropeBusinessUnit;
    }
    
    public async Task<bool> DeleteAsync(int clientEuropeId, int businessUnitId, ContextSession session)
    {
        if (clientEuropeId < 1 || businessUnitId < 1) return false;

        var cebu = await _context.ClientEuropeBusinessUnits
            .Where(cebu => cebu.ClientId == clientEuropeId && cebu.BusinessUnitId == businessUnitId && !cebu.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (cebu == null) return false;

        cebu.IsDeleted = true;
        cebu.UpdatedAt = DateTime.Now;
        cebu.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        await _context.SaveChangesAsync();
        
        cebu = await GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, session);
        if (cebu != null) return false;
        
        return true;
    }
}