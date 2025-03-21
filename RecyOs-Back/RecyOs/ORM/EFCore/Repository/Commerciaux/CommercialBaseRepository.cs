// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialRepository.cs
// Created : 2024/03/26 - 15:22
// Updated : 2024/03/26 - 15:22

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.Extensions;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

#nullable enable
public class CommercialBaseRepository : BaseDeletableRepository<Commercial, DataContext>, ICommercialBaseRepository
{
    private readonly DataContext _context;
    public CommercialBaseRepository(DataContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Commercial> CreateAsync(Commercial entity, ContextSession session)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IReadOnlyList<Commercial>> GetListAsync(bool includeDeleted = false)
    {
        return await GetEntities(new ContextSession())
            .ToListAsync();
    }
    
    public async Task<(IEnumerable<Commercial>, int)> GetFilteredListAsync(CommercialFilter filter, ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted)
            .ApplyFilter(filter);
        
        var totalCount = await query.CountAsync();
        var resultList = await query
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize)
            .ToArrayAsync();

        return (resultList, totalCount);
    }

    public async Task<Commercial> UpdateAsync(Commercial entity, ContextSession session)
    {
        var context = GetContext(session);

        var existingEntity = await context.Commerciaux.FindAsync(entity.Id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await context.SaveChangesAsync();
        }
        
        return existingEntity;
    }

    public async Task<(IEnumerable<object>, int)> GetClientsByCommercialIdAsyncWithCount(int id, ClientByCommercialFilter filter)
    {
        List<object> combinedEntities = new List<object>();

        var etablissementClientsQuery = _context.EtablissementClient
            .AsNoTracking()
            .Where(ec => ec.CommercialId == id && !ec.IsDeleted && ec.Client)
            .Include(ec => ec.EntrepriseBase)
            .ThenInclude(ef => ef.EntrepriseNDCover)
            .ApplyClientByCommercialUnitFilter<EtablissementClient>(filter)
            .ApplyClientByCommercialSearchFilter<EtablissementClient>(filter);
        
        var etablissementClients = await etablissementClientsQuery.ToListAsync();
        combinedEntities.AddRange(etablissementClients);

        
        var clientsEuropeQuery = _context.ClientEurope
            .AsNoTracking()
            .Where(ce => ce.CommercialId == id && !ce.IsDeleted && ce.Client)
            .ApplyClientByCommercialUnitFilter<ClientEurope>(filter)
            .ApplyClientByCommercialSearchFilter<ClientEurope>(filter);

        var clientsEurope = await clientsEuropeQuery.ToListAsync();
        combinedEntities.AddRange(clientsEurope);
        
        var totalCount = combinedEntities.Count;
        var resultList = combinedEntities
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize)
            .ToArray();

        return (resultList, totalCount);
    }
    
    #nullable enable
    public async Task<Commercial?> GetByMailAsync(string mail, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Email == mail)
            .FirstOrDefaultAsync();
    }
    #nullable disable
    
    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        var existingEntity = await _context.Commerciaux.FindAsync(id);
        if (existingEntity == null) return false;
        await Delete(id, session);
        return true;
    }

    public async Task<Commercial> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }
}