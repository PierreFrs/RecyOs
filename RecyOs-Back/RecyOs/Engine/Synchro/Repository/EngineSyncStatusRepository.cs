using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Repository;

public class EngineSyncStatusRepository : IEngineSyncStatusRepository
{
    private readonly DataContext _context;
    
    public EngineSyncStatusRepository(IDataContextEngine ctx)
    {
        _context = ctx.GetContext();
    }
    
    public EngineSyncStatus GetByModuleName(string prmValue)
    {
        return _context.EngineSyncStatus.AsNoTracking().FirstOrDefault(e => e.ModuleName == prmValue);
    }

    public IList<EngineSyncStatus> GetEnabledModules()
    {
        return _context.EngineSyncStatus.AsNoTracking().Where(e => e.ModuleEnabled).ToList();
    }
}