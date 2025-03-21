using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Repository;

public class EngineParametterRepository : IEngineParametterRepository
{
    private readonly DataContext _context;
    
    public EngineParametterRepository(IDataContextEngine dataContextEngine)
    {
        _context = dataContextEngine.GetContext();
    }
    
    public async Task<Parameter> GetAsync(int id, bool includeDeleted = false)
    {
        return await _context.Parameters
            .Where(x => x.Id == id && (includeDeleted || !x.IsDeleted))
            .FirstOrDefaultAsync();
    }
    
    public async Task<Parameter> GetByNomAsync(string module, string nom, bool includeDeleted = false)
    {
        return await _context.Parameters
            .Where(x => x.Module == module && x.Nom == nom && (includeDeleted || !x.IsDeleted))
            .FirstOrDefaultAsync();
    }
    
    public async Task<Parameter> CreateAsync(Parameter prm)
    {
        var ret = await _context.Parameters.AddAsync(prm);
        await _context.SaveChangesAsync();
        return ret.Entity;
    }
}