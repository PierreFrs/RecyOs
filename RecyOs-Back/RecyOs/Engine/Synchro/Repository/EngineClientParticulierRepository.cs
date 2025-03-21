using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Repository;

public class EngineClientParticulierRepository : IEngineClientParticulierRepository
{
    private readonly DataContext _context;
    
    public EngineClientParticulierRepository(IDataContextEngine context)
    {
        _context = context.GetContext();
    }

    /// <summary>
    /// Retrieves a list of ClientParticulier entities created for marketing purposes.
    /// </summary>
    /// <returns>A list of ClientParticulier entities.</returns>
    private IList<ClientParticulier> getCreatedClientParticuliersMkgt()
    {
        DateTime? lastCreate = getLastCreDateTimeMkgt();
        DateTime now = DateTime.Now;
        
        var res =_context.ClientParticuliers.AsNoTracking()
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "") && ec.DateCreMkgt > lastCreate 
                                                                    && ec.DateCreMkgt <= now && !ec.IsDeleted)
            .ToList();
        
        setLastCreDateTimeMkgt(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves a list of ClientParticulier entities need shipper ID to be created.
    /// </summary>
    /// <returns>A list of ClientParticulier entities.</returns>
    private IList<ClientParticulier> getCreatedClientParticuliersShipper()
    {
        var res = _context.ClientParticuliers.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc == null || ec.IdShipperDashdoc == 0) && ec.CodeMkgt != null 
                && ec.CodeMkgt != "" && ec.IdOdoo != null && ec.IdOdoo != "" && !ec.IsDeleted)
            .ToList();
        
        return res;
    }

    /// <summary>
    /// Retrieves a list of updated ClientParticulier entities with a non-empty CodeMkgt
    /// that have been updated after the last update date and before the current date.
    /// </summary>
    /// <returns>A list of updated ClientParticulier entities.</returns>
    public IList<ClientParticulier> GetUpdatedClientParticulierMkgt()
    {
        DateTime? lastUpdate = getLastUpdDateTimeMkgt();
        DateTime now = DateTime.Now;
        
        var res = _context.ClientParticuliers.AsNoTracking()
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "") && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now
                         && !ec.IsDeleted)
            .ToList();
        
        setLastUpdDateTimeMkgt(now);
        return res;
    }

    /// <summary>
    /// Retrieves the updated ClientParticulier from Odoo.
    /// </summary>
    /// <returns>
    /// A list of updated ClientParticulier entities.
    /// </returns>
    public IList<ClientParticulier> GetUpdatedClientParticulierOdoo()
    {
        DateTime? lastUpdate = getLastUpdDateTimeOdoo();
        DateTime now = DateTime.Now;
        
        var res = _context.ClientParticuliers.AsNoTracking()
            .Where(ec => (ec.IdOdoo != null && ec.IdOdoo != "") && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now
                         && !ec.IsDeleted)
            .ToList();
        
        setLastUpdDateTimeOdoo(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves the updated ClientParticulier entities from the Shipper.
    /// </summary>
    /// <returns>A list of updated ClientParticulier entities.</returns>
    public IList<ClientParticulier> GetUpdatedClientParticulierShipper()
    {
        DateTime? lastUpdate = getLastUpdDateTimeShipper();
        DateTime now = DateTime.Now;
        
        var res = _context.ClientParticuliers.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc != null && ec.IdShipperDashdoc != null && ec.IdShipperDashdoc > 0)
                         && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now && !ec.IsDeleted)
            .ToList();
        
        setLastUpdDateTimeShipper(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves the last creation date and time for MkgtClientParticulierModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last creation date and time as <see cref="DateTime"/> if found, otherwise null.</returns>
    private DateTime? getLastCreDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName == "MkgtClientParticulierModule")
            .Select(ess => ess.LastCreate).FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last update datetime for the MkgtClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>
    /// The last update datetime for the MkgtClientParticulierModule, or null if no record is found.
    /// </returns>
    private DateTime? getLastUpdDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtClientParticulierModule"))
            .Select(ess => ess.LastUpdate).FirstOrDefault();
    }
    
    /// <summary>
    /// Sets the last created date and time for the MkgtClientModule.
    /// </summary>
    /// <param name="prmValue">The date and tigetLastCreSateTimeMme to set as the last created date and time.</param>
    private void setLastCreDateTimeMkgt(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientParticulierModule"))
            .FirstOrDefault()
            .LastCreate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last update date and time for the MkgtClientModule.
    /// </summary>
    /// <param name="prmValue">The value to be set as the last update date and time.</param>
    private void setLastUpdDateTimeMkgt(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientParticulierModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Returns the last update date and time of the Odoo client module.
    /// </summary>
    /// <returns>The last update date and time if available, otherwise null.</returns>
    private DateTime? getLastUpdDateTimeOdoo()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("OdooClientParticulierModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Sets the last update date and time for the Odoo Client Module.
    /// </summary>
    /// <param name="prmValue">The date and time to set as the last update.</param>
    private void setLastUpdDateTimeOdoo(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooClientParticulierModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Get the last update date and time for the MkgtShipperClientParticulierModule.
    /// </summary>
    /// <returns>The last update date and time if available, otherwise null.</returns>
    private DateTime? getLastUpdDateTimeShipper()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtShipperClientParticulierModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Sets the last update date and time for the MkgtShipperClientParticulierModule.
    /// </summary>
    /// <param name="prmValue">The date and time to set the last update.</param>
    private void setLastUpdDateTimeShipper(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientParticulierModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Retrieves the list of created entities for a given module.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>The list of created entities as instances of ClientParticulier.</returns>
    public IList<ClientParticulier> GetCreatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtClientParticulierModule":
                return getCreatedClientParticuliersMkgt();
            case "MkgtShipperClientParticulierModule":
                return getCreatedClientParticuliersShipper();
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Retrieves a list of updated entities for the specified module name.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>A list of updated entities.</returns>
    public IList<ClientParticulier> GetUpdatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtClientParticulierModule":
                return GetUpdatedClientParticulierMkgt();
            case "OdooClientParticulierModule":
                return GetUpdatedClientParticulierOdoo();
            case "MkgtShipperClientParticulierModule":
                return GetUpdatedClientParticulierShipper();
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Updates the destination IDs for the client particulier entities.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <param name="items">The list of client particulier entities.</param>
    /// <returns>The list of client particulier entities with updated destination IDs.</returns>
    public IList<ClientParticulier> CallBackDestIdCreation(string moduleName, IList<ClientParticulier> items)
    {
        switch (moduleName)
        {
            case "MkgtClientParticulierModule":
                throw new NotSupportedException();
            case "OdooClientParticulierModule":
                throw new NotSupportedException();
            case "MkgtShipperClientParticulierModule":
                return updateShipperIds(items);
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Updates the Shipper IDs for the client particulier entities.
    /// </summary>
    /// <param mame="items">The list of client particulier entities.</param>
    /// <returns>The list of client particulier entities with updated Shipper IDs.</returns>
    private IList<ClientParticulier> updateShipperIds(IList<ClientParticulier> items)
    {
        foreach (var item in items)
        {
            var clientParticulier = _context.ClientParticuliers.SingleOrDefault(ec => ec.CodeMkgt == item.CodeMkgt);
            if (clientParticulier != null)
            {
                clientParticulier.IdShipperDashdoc = item.IdShipperDashdoc;
                _context.SaveChanges();
            }
        }
        return items;
    }
}