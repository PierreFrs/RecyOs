using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecyOs.Engine.Interfaces;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Repository;

/// <summary>
/// Represents a repository for accessing and manipulating Engine Europe client data.
/// </summary>
public class EngineEuropeClientRepository :  IEngineEuropeClientRepository
{
    private readonly DataContext _context;
        
    public EngineEuropeClientRepository(IDataContextEngine ctx)
    {
        _context = ctx.GetContext();
    }


    /// <summary>
    /// Retrieves a list of created entities from the ClientEurope table based on the Mkgt code and creation date.
    /// </summary>
    /// <returns>A list of ClientEurope objects that have a non-null and non-empty Mkgt code, and were created after the last recorded creation date and before the current date.</returns>
    private IList<ClientEurope> GetCreatedEntitiesMkgt()
    {
        DateTime? lastCreate = getLastCreDateTimeMkgt();
        DateTime now = DateTime.Now;

        var res =_context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "")
                                          && ec.DateCreMkgt > lastCreate && ec.DateCreMkgt <= now && !ec.IsDeleted)
            .Include(x => x.Commercial)
            .Include(x => x.FactorClientEuropeBus)
            .ThenInclude(y => y.BusinessUnit).ToList();
        
        setLastCreDateTimeMkgt(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves a list of ClientEurope need shipper ID to be created.
    /// </summary>
    /// <returns>A list of ClientEurope objects that have a null or 0 IdShipperDashdoc and MkgtCode</returns>
    private IList<ClientEurope> GetCreatedEntitiesMkgtShipper()
    {
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc == null || ec.IdShipperDashdoc == 0) 
                         && (ec.CodeMkgt != null && ec.CodeMkgt != "")&& ec.IdOdoo != null 
                         && ec.IdOdoo != "" && !ec.IsDeleted)
            .ToList();
        return res;
    }

    /// <summary>
    /// Retrieves a list of client entities created in the GPI system within a specified time range.
    /// </summary>
    /// <returns>
    /// A list of <see cref="ClientEurope"/> instances representing the created client entities.
    /// </returns>
    private IList<ClientEurope> GetCreatedEntitiesGpi()
    {
        DateTime? lastCreate = getLastCreDateTimeGpi();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.CodeGpi != null && ec.CodeGpi != "") && ec.DateCreGpi > lastCreate 
                                                                  && ec.DateCreGpi <= now && !ec.IsDeleted)
            .ToList();
        setLastCreDateTimeGpi(now);
        return res;
    }
    

    /// <summary>
    /// Retrieves a list of updated ClientEurope entities since the last update date and time.
    /// </summary>
    /// <returns>
    /// A list of updated ClientEurope entities.
    /// </returns>
    private IList<ClientEurope> GetUpdatedEntitiesMkgt()
    {
        DateTime? lastUpdate = getLastUpdDateTimeMkgt();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "") && ec.UpdatedAt > lastUpdate 
                                                                    && ec.UpdatedAt <= now && !ec.IsDeleted)
            .Include(x => x.Commercial)
            .Include(x => x.FactorClientEuropeBus)
            .ThenInclude(y => y.BusinessUnit)
            .ToList();
        setLastUpdDateTimeMkgt(now);
        return res;
    }

    /// <summary>
    /// Retrieves the updated entities from Odoo based on the last update time.
    /// </summary>
    /// <returns>A list of updated entities of type ClientEurope.</returns>
    private IList<ClientEurope> GetUpdatedEntitiesOdoo()
    {
        DateTime? lastUpdate = getLastUpdDateTimeOdoo();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.IdOdoo != null && ec.IdOdoo != "") && ec.UpdatedAt > lastUpdate 
                                                                && ec.UpdatedAt <= now && !ec.IsDeleted)
            .ToList();
        setLastUpdDateTimeOdoo(now);
        return res;
    }

    /// <summary>
    /// Retrieves a list of updated entities from the ClientEurope table that have a non-null and non-empty Gpi code and were updated after the last recorded update date and before the current date.
    /// </summary>
    /// <returns>A list of ClientEurope objects that meet the specified criteria.</returns>
    private IList<ClientEurope> GetUpdatedEntitiesGpi()
    {
        DateTime? lastUpdate = getLastUpdDateTimeGpi();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.CodeGpi != null && ec.CodeGpi != "") && ec.UpdatedAt > lastUpdate 
                                                                  && ec.UpdatedAt <= now && !ec.IsDeleted)
            .ToList();
        setLastUpdDateTimeGpi(now);
        return res;
    }

    /// <summary>
    /// Retrieves a list of updated entities from the ClientEurope table in the HubSpot module based on the last recorded update date and the current date.
    /// </summary>
    /// <returns>A list of ClientEurope objects that have a non-null and non-empty IdHubspot, and have been updated after the last recorded update date and before the current date.</returns>
    private IList<ClientEurope> GetUpdatedEntitiesHubSpot()
    {
        DateTime? lastUpdate = getLastUpdDateTimeHubSpot();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.IdHubspot != null && ec.IdHubspot != "") 
                         && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now && !ec.IsDeleted)
            .Include(x => x.Commercial)
            .ToList();
        setLastUpdDateTimeHubSpot(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves the updated entities from Dashdoc based on the last update time.
    /// </summary>
    /// <returns>A list of updated entities of type ClientEurope.</returns>
    private IList<ClientEurope> GetUpdatedEntitiesDashDoc()
    {
        DateTime? lastUpdate = getLastUpdDateTimeDashDoc();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.IdDashdoc != null && ec.IdDashdoc != 0) 
                         && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now && !ec.IsDeleted)
            .ToList();
        setLastUpdDateTimeDashDoc(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves a list of updated ClientEurope entities with a non-empty IdShipperDashdoc
    /// </summary>
    /// <returns>A list of updated entities of type ClientEurope.</returns>
    private IList<ClientEurope> GetUpdatedEntitiesMkgtShipper()
    {
        DateTime? lastUpdate = getLastUpdDateTimeMkgtShipper();
        DateTime now = DateTime.Now;
        var res = _context.ClientEurope.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc != null && ec.IdShipperDashdoc != 0) 
                         && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now && !ec.IsDeleted)
            .ToList();
        setLastUpdDateTimeMkgtShipper(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves the last creation DateTime for MKGT Europe Client Module from the Engine Sync Status.
    /// </summary>
    /// <returns>The last creation DateTime for MKGT Europe Client Module. Null if not found.</returns>
    private DateTime? getLastCreDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName == "MkgtEuropeClientModule")
            .Select(ess => ess.LastCreate).FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last update date and time for the MkgtEuropeClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the MkgtEuropeClientModule, or null if no update has been recorded.</returns>
    private DateTime? getLastUpdDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtEuropeClientModule"))
            .Select(ess => ess.LastUpdate).FirstOrDefault();
    }

    /// <summary>
    /// Sets the LastCreate field of the EngineSyncStatus entity for the MkgtEuropeClientModule.
    /// </summary>
    /// <param name="prmValue">The DateTime value to be set as LastCreate.</param>
    private void setLastCreDateTimeMkgt(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtEuropeClientModule"))
            .FirstOrDefault()
            .LastCreate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last update date and time for the MkgtEuropeClientModule in the EngineSyncStatus table.
    /// </summary>
    /// <param name="prmValue">The value representing the updated date and time.</param>
    private void setLastUpdDateTimeMkgt(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }


    /// <summary>
    /// Retrieves the last update datetime from the Odoo Europe Client Module in the Engine Sync Status.
    /// </summary>
    /// <returns>
    /// The last update datetime if available; otherwise, null.
    /// </returns>
    private DateTime? getLastUpdDateTimeOdoo()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("OdooEuropeClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }


    /// <summary>
    /// Sets the last update date and time for the Odoo Europe Client Module.
    /// </summary>
    /// <param name="prmValue">The date and time value to set.</param>
    private void setLastUpdDateTimeOdoo(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Retrieves the last update date and time for the GpiEuropeClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time of the GpiEuropeClientModule as DateTime object, or null if no record found.</returns>
    private DateTime? getLastUpdDateTimeGpi()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Returns the last created date and time for the GpiEuropeClientModule.
    /// </summary>
    /// <returns>The last created date and time as a nullable DateTime.</returns>
    private DateTime? getLastCreDateTimeGpi()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .Select(ess => ess.LastCreate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last update date and time for the HubSpotEuropeClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the HubSpotEuropeClientModule, or null if no update records exist.</returns>
    private DateTime? getLastUpdDateTimeHubSpot()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("HubSpotEuropeClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Retrieves the last update date and time for the MkgtShipperEuropeClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the MkgtShipperEuropeClientModule</returns>
    private DateTime? getLastUpdDateTimeMkgtShipper()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtShipperEuropeClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Sets the last update date and time for the MkgtShipperEuropeClientModule in the EngineSyncStatus table.
    /// </summary>
    /// <param name="prmDateTime">The date and time to be set.</param>
    private void setLastUpdDateTimeMkgtShipper(DateTime prmDateTime)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmDateTime;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last create date and time for GPI Europe Client Module in the system.
    /// </summary>
    /// <param name="prmDateTime">The date and time to be set.</param>
    private void setLastCreDateTimeGpi(DateTime prmDateTime)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault()
            .LastCreate = prmDateTime;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last update date and time for GPI Europe Client Module.
    /// </summary>
    /// <param name="prmValue">The date and time to be set as the last update.</param>
    private void setLastUpdDateTimeGpi(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last update date and time for the HubSpotEuropeClientModule in the EngineSyncStatus table.
    /// </summary>
    /// <param name="prmValue">The value to set as the last update date and time.</param>
    private void setLastUpdDateTimeHubSpot(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("HubSpotEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Retrieves the last update date and time for the DashDocEuropeClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the DashDocEuropeClientModule, or null if no update records exist.</returns>
    private DateTime? getLastUpdDateTimeDashDoc()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("DashDocEuropeClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Sets the last update date and time for the DashDocEuropeClientModule in the EngineSyncStatus table.
    /// </summary>
    /// <param name="prmValue">The value to set as the last update date and time.</param>
    private void setLastUpdDateTimeDashDoc(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("DashDocEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Retrieves the updated entities based on the provided module name.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>An IList of ClientEurope objects if the module name is valid; otherwise, null.</returns>
    public IList<ClientEurope> GetUpdatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtEuropeClientModule":
                return GetUpdatedEntitiesMkgt();
               
            case "OdooEuropeClientModule":
                return GetUpdatedEntitiesOdoo();
            
            case "GpiEuropeClientModule":
                return GetUpdatedEntitiesGpi();
            
            case "HubSpotEuropeClientModule":
                return GetUpdatedEntitiesHubSpot();
            
            case "DashDocEuropeClientModule":
                return GetUpdatedEntitiesDashDoc();
            
            case "MkgtShipperEuropeClientModule":
                return GetUpdatedEntitiesMkgtShipper();
                
            default:
                return null;
        }
    }

    /// <summary>
    /// Retrieves a list of created entities based on the given module name.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>
    /// If the module name is "MkgtEuropeClientModule", then the method returns a list of ClientEurope entities created.
    /// If the module name is "OdooEuropeClientModule", then the method returns null, as Odoo requires a direct write for creation.
    /// If the module name is "GpiEuropeClientModule", then the method returns a list of ClientEurope entities created using the Gpi module.
    /// For any other module name, the method returns null.
    /// </returns>
    public IList<ClientEurope> GetCreatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtEuropeClientModule":
                return GetCreatedEntitiesMkgt();
               
            case "OdooEuropeClientModule":
                return null;  // Odoo need direct write for creation so this will return null everytime
            
            case "GpiEuropeClientModule":
                return GetCreatedEntitiesGpi();
            
            case "MkgtShipperEuropeClientModule":
                return GetCreatedEntitiesMkgtShipper();
                
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Updates the destination IDs for the client Europe entities.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <param name="items">The list of client Europe entities.</param>
    /// <returns>The list of client Europe entities with updated destination IDs.</returns>
    public IList<ClientEurope> CallBackDestIdCreation(string moduleName, IList<ClientEurope> items)
    {
        switch (moduleName)
        {
            case "MkgtEuropeClientModule":
                throw new NotSupportedException();
            case "OdooEuropeClientModule":
                throw new NotSupportedException();
            case "GpiEuropeClientModule":
                throw new NotSupportedException();
            case "HubSpotEuropeClientModule":
                throw new NotSupportedException();
            case "DashDocEuropeClientModule":
                throw new NotSupportedException();
            case "MkgtShipperEuropeClientModule":
                return updateShipperIds(items);
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Updates the Shipper IDs for the client Europe entities.
    /// </summary>
    /// <param name="items">The list of client Europe entities.</param>
    /// <returns>The list of client Europe entities with updated Shipper IDs.</returns>
    private IList<ClientEurope> updateShipperIds(IList<ClientEurope> items)
    {
        foreach (var item in items)
        {
            var client = _context.ClientEurope
                .SingleOrDefault(ec => ec.CodeMkgt == item.CodeMkgt);
            if (client != null)
            {
                client.IdShipperDashdoc = item.IdShipperDashdoc;
                _context.SaveChanges();
            }
        }
        return items;
    }
}