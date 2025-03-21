using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Filters.Extensions;
using RecyOs.ORM.Interfaces.IParameters;

namespace RecyOs.ORM.EFCore.Repository.Application;

public class ParameterRepository: BaseDeletableRepository<Parameter, DataContext>, IParameterRepository<Parameter>
{
    public ParameterRepository(DataContext context) : base(context)
    {
    }
    
    public Task<Parameter> GetAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return GetEntities(session, includeDeleted)
            .AsNoTracking()
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Parameter> UpdateAsync(Parameter parameter, ContextSession session)
    {
        var context = GetContext(session);

        // Chercher l'entité existante dans le contexte (par clé primaire)
        var existingEntity = await context.Parameters.FindAsync(parameter.Id);

        if (existingEntity != null)
        {
            // L'entité existe déjà, mise à jour de ses propriétés
            context.Entry(existingEntity).CurrentValues.SetValues(parameter);
            context.Entry(existingEntity).State = EntityState.Modified;
        }
        else
        {
            // L'entité n'existe pas, donc on l'ajoute
            context.Parameters.Add(parameter);
        }
      
        await context.SaveChangesAsync();
        return parameter;
    }

    public async Task<Parameter> CreateAsync(Parameter parameter, ContextSession session)
    {
        var context = GetContext(session);
        context.Entry(parameter).State = EntityState.Added;
        await context.SaveChangesAsync();
        return parameter; 
    }

    public async Task<bool> DeleteAsync(int id, ContextSession session)
    {
        var context = GetContext(session);
        var parameter = await context.Parameters.FindAsync(id);
        parameter.IsDeleted = true;
        context.Entry(parameter).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<(IEnumerable<Parameter>, int)> GetDataForGrid(ParameterFilter filter, ContextSession session, bool includeDeleted = false)
    {
        var query = GetEntities(session, includeDeleted).ApplyFilter(filter);
        return ( await query
            .Skip(filter.PageSize * (filter.PageNumber))
            .Take(filter.PageSize)
            .ToArrayAsync(), await query.CountAsync());
    }
    
    public async Task<Parameter> GetByNom(string module, string nom, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .AsNoTracking()
            .Where(obj => obj.Module == module && obj.Nom == nom)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<string>> GetAllModulesAsync(ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()
            .Select(obj => obj.Module)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IList<Parameter>> GetAllByModuleAsync(string module, ContextSession session)
    {
        return await GetEntities(session)
            .AsNoTracking()
            .Where(obj => obj.Module == module)
            .ToListAsync();
    }

    public async Task<Parameter> GetByNomAsync(string module, string nom, ContextSession session, bool includeDeleted = false)
    {
        return await GetEntities(session, includeDeleted)
            .AsNoTracking()
            .Where(obj => obj.Module == module && obj.Nom == nom)
            .FirstOrDefaultAsync();
    }
}