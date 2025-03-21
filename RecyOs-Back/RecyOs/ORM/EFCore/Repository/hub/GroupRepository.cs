using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Filters.Extensions;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class GroupRepository : BaseDeletableRepository<Group, DataContext>, IGroupRepository
{
    private readonly DataContext _context;
    public GroupRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Group> CreateAsync(Group group, ContextSession session)
    {
        try {
            var context = GetContext(session);
            context.Add(group);
            await context.SaveChangesAsync();
            var newGroup = await context.Groups.FirstOrDefaultAsync(x => x.Id == group.Id);
            return newGroup;
        }
        catch (Exception ex)
        {
            // Log the exception details
            throw new Exception($"Error creating group: {ex.Message}", ex);
        }
    }

    public async Task<IReadOnlyList<Group>> GetListAsync(bool includeDeleted = false)
    {
        return await GetEntities(new ContextSession(), includeDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Group> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .Where(obj => obj.Id == id)
            .Include(g => g.ClientEuropes.Where(ce => !ce.IsDeleted))
            .Include(g => g.EtablissementClients.Where(ec => !ec.IsDeleted))
            .FirstOrDefaultAsync();
    }

    public async Task<Group> GetByNameAsync(string name, ContextSession session)
    {
        return await GetEntities(session)
            .Where(obj => obj.Name == name)
            .FirstOrDefaultAsync();
    }

    public async Task<Group> UpdateAsync(Group group, ContextSession session)
    {
        try
        {
            var context = GetContext(session);
            var groupToUpdate = await context.Groups.FindAsync(group.Id);
            if (groupToUpdate == null) throw new NullReferenceException();

            context.Entry(groupToUpdate).CurrentValues.SetValues(group);
            await context.SaveChangesAsync();
            return groupToUpdate;
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        await Delete(id, session);
        return true;
    }

    public async Task<(IEnumerable<Group>, int)> GetFilteredListWithClientsAsync(GroupFilter filter, ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted)
            .Include(g => g.ClientEuropes.Where(ce => !ce.IsDeleted))
            .Include(g => g.EtablissementClients.Where(ec => !ec.IsDeleted))
            .AsNoTracking()
            .ApplyFilter(filter);

        // First, materialize the count query
        var totalCount = await query.CountAsync();

        // Then, execute the main query with includes
        var resultList = await query
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize)
            .ToArrayAsync();  // Use ToListAsync instead of ToArrayAsync

        return (resultList, totalCount);
    }
} 