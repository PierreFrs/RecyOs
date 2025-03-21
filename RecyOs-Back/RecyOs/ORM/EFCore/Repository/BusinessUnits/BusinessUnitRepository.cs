using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class BusinessUnitRepository : BaseRepository<BusinessUnit, DataContext>, IBusinessUnitRepository<BusinessUnit>
{
    private readonly DataContext _context;
    public BusinessUnitRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<BusinessUnit>> GetListAsync(ContextSession session, bool includeDeleted = false)
    {
        return await _context.BusinessUnits.ToListAsync();
    }

    public async Task<BusinessUnit> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false)
    {
        return await _context.BusinessUnits
            .Where(obj => obj.Id == id)
            .FirstOrDefaultAsync();
    }
}