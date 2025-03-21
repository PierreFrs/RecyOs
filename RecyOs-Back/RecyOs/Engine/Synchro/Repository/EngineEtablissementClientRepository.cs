//  EtablissementClientRepository.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecyOs.Engine.Interfaces;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Repository;

public class EngineEtablissementClientRepository : IEngineEtablissementClientRepository
{
    private readonly DataContext _context;
    private readonly IConfiguration _config;
    public EngineEtablissementClientRepository(
        IDataContextEngine ctx,
        IConfiguration config
        )
    {
        _context = ctx.GetContext();
        _config = config;
    }


    /// <summary>
    /// Retrieves a list of EtablissementClient entities created for marketing purposes.
    /// </summary>
    /// <returns>A list of EtablissementClient entities.</returns>
    private IList<EtablissementClient> GetCreatedEntitiesMkgt()
    {
        DateTime? lastCreate = getLastCreDateTimeMkgt();
        DateTime now = DateTime.Now;

        var res =_context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "") && ec.DateCreMKGT > lastCreate && ec.DateCreMKGT <= now
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase)
            .Include(x => x.Commercial)
            .Include(x => x.FactorClientFranceBus)
            .ThenInclude(y => y.BusinessUnit).ToList();
        
        setLastCreDateTimeMkgt(now);
        return res;
    }

    /// <summary>
    /// Retrieves a list of newly created entities from the EtablissementClient table based on the provided criteria. </summary>
    /// <returns>Returns a list of EtablissementClient objects that match the specified conditions. </returns>
    private IList<EtablissementClient> GetCreatedEntitiesGpi()
    {
        DateTime? lastCreate = getLastCreDateTimeGpi();
        DateTime now = DateTime.Now;
        var res = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.CodeGpi != null && ec.CodeGpi != "") && ec.DateCreGpi > lastCreate && ec.DateCreGpi <= now
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase).ToList();
        setLastCreDateTimeGpi(now);
        return res;
    }
    
    /// <summary>
    /// Retrieves a list of EtablissementClient need shipper ID to be created.
    /// </summary>
    /// <returns>Returns a list of EtablissementClient objects that match the specified conditions. </returns>
    private IList<EtablissementClient> GetCreatedEntitiesMkgtShipper()
    {
        var res = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc == null || ec.IdShipperDashdoc == 0) && ec.CodeMkgt != null 
                && ec.CodeMkgt != "" && ec.IdOdoo != null && ec.IdOdoo != "" 
                && ec.EntrepriseBase.NumeroTvaIntracommunautaire != null 
                && ec.EntrepriseBase.NumeroTvaIntracommunautaire != "" && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase)
            .ToList();
        return res;
    }

    /// <summary>
    /// Retrieves a list of updated EtablissementClient entities with a non-empty CodeMkgt
    /// that have been updated after the last update date and before the current date.
    /// </summary>
    /// <returns>A list of updated EtablissementClient entities.</returns>
    public IList<EtablissementClient> GetUpdatedEntitiesMkgt()
    {
        DateTime? lastUpdate = getLastUpdDateTimeMkgt();
        DateTime now = DateTime.Now;
        var res = _context.EtablissementClient
            .Where(ec => (ec.CodeMkgt != null && ec.CodeMkgt != "") && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase)
            .Include(x => x.Commercial)
            .Include(x => x.FactorClientFranceBus)
            .ThenInclude(y => y.BusinessUnit).AsNoTracking().ToList();
        setLastUpdDateTimeMkgt(now);
        return res;
    }

    /// <summary>
    /// Retrieves the updated entities from Odoo.
    /// </summary>
    /// <returns>
    /// A list of updated EtablissementClient entities.
    /// </returns>
    public IList<EtablissementClient> GetUpdatedEntitiesOdoo()
    {
        DateTime? lastUpdate = getLastUpdDateTimeOdoo();
        DateTime now = DateTime.Now;
        var res = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.IdOdoo != null && ec.IdOdoo != "") && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now 
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase).ToList();
        setLastUpdDateTimeOdoo(now);
        return res;
    }

    /// <summary>
    /// Retrieves the updated entities of EtablissementClient from the database based on the last update time for GPI code.
    /// </summary>
    /// <returns>
    /// A list of EtablissementClient objects that have been updated since the last update time for GPI code.
    /// </returns>
    public IList<EtablissementClient> GetUpdatedEntitiesGpi()
    {
        DateTime? lastUpdateGpi = getLastUpdDateTimeGpi();
        DateTime nowGpi = DateTime.Now;
        var resGpi = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.CodeGpi != null && ec.CodeGpi != "") && ec.UpdatedAt > lastUpdateGpi 
                                                                  && ec.UpdatedAt <= nowGpi
                                                                  && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase).ToList();
        setLastUpdDateTimeGpi(nowGpi);
        return resGpi;
    }

    /// <summary>
    /// Retrieves a list of EtablissementClient entities that have been updated in the HubSpot system.
    /// </summary>
    /// <returns>A list of EtablissementClient entities.</returns>
    public IList<EtablissementClient> GetUpdatedEntitiesHubSpot()
    {
        DateTime? lastUpdate = getLastUpdDateTimeHubSpot();
        DateTime now = DateTime.Now;
    
        var etablissementClients = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.IdHubspot != null && ec.IdHubspot != "") && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase)
            .Include(x => x.Commercial)
            .Include(x => x.EntrepriseBase.EntrepriseCouverture)
            .Include(x => x.EntrepriseBase.EntrepriseNDCover)
            .ToList();

        var filteredFiches = _context.EtablissementFiche.AsNoTracking()
            .Where(ef => etablissementClients.Select(ec => ec.Siret).Contains(ef.Siret))
            .ToList();

        foreach (var ec in etablissementClients)
        {
            ec.EtablissementFiche = filteredFiches.Find(f => f.Siret == ec.Siret);
        }

        setLastUpdDateTimeHubSpot(now);
        return etablissementClients;
    }
    
    /// <summary>
    /// Retrieves a list of updated EtablissementClient entities with a non-empty DashDoc code
    /// that have been updated after the last update date and before the current date.
    /// </summary>
    /// <returns>A list of updated EtablissementClient entities.</returns>
    public IList<EtablissementClient> GetUpdatedEntitiesDashDoc()
    {
        DateTime? lastUpdate = getLastUpdDateTimeDashDoc();
        DateTime now = DateTime.Now;
        var res = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.IdDashdoc != null && ec.IdDashdoc != 0) && ec.UpdatedAt > lastUpdate && ec.UpdatedAt <= now
                         && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase)
            .Include(x => x.Commercial)
            .Include(x => x.FactorClientFranceBus)
            .ThenInclude(y => y.BusinessUnit).ToList();
        setLastUpdDateTimeDashDoc(now);
        return res;
    }
    
    
    /// <summary>
    /// Retrieves a list of updated EtablissementClient entities with a non-empty IdShipperDashdoc
    /// that have been updated after the last update date and before the current date.
    /// </summary>
    /// <returns>A list of updated EtablissementClient entities.</returns>
    public IList<EtablissementClient> GetUpdatedEntitiesMkgtShipper()
    {
        DateTime? lastUpdate = getLastUpdDateTimeMkgtShipper();
        DateTime now = DateTime.Now;
        var res = _context.EtablissementClient.AsNoTracking()
            .Where(ec => (ec.IdShipperDashdoc != null && ec.IdShipperDashdoc != 0) && ec.UpdatedAt > lastUpdate 
                && ec.UpdatedAt <= now && !ec.IsDeleted)
            .Include(x => x.EntrepriseBase).ToList();
        setLastUpdDateTimeMkgtShipper(now);
        return res;
    }


    /// <summary>
    /// Retrieves the last creation date and time for MkgtClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last creation date and time as <see cref="DateTime"/> if found, otherwise null.</returns>
    private DateTime? getLastCreDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName == "MkgtClientModule")
            .Select(ess => ess.LastCreate).FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last update datetime for the MkgtClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>
    /// The last update datetime for the MkgtClientModule, or null if no record is found.
    /// </returns>
    private DateTime? getLastUpdDateTimeMkgt()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtClientModule"))
            .Select(ess => ess.LastUpdate).FirstOrDefault();
    }

    /// <summary>
    /// Sets the last created date and time for the MkgtClientModule.
    /// </summary>
    /// <param name="prmValue">The date and time to set as the last created date and time.</param>
    private void setLastCreDateTimeMkgt(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientModule"))
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
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientModule"))
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
            .Where(ess => ess.ModuleName.Equals("OdooClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Sets the last update date and time for the Odoo Client Module.
    /// </summary>
    /// <param name="prmValue">The date and time to set as the last update.</param>
    private void setLastUpdDateTimeOdoo(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Returns the last update date and time of the DashDoc client module.
    /// </summary>
    /// <returns>The last update date and time if available, otherwise null.</returns>
    private DateTime? getLastUpdDateTimeDashDoc()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("DashDocClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Sets the last update date and time for the DashDoc Client Module.
    /// </summary>
    /// <param name="prmValue">The date and time to set as the last update.</param>
    private void setLastUpdDateTimeDashDoc(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("DashDocClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Retrieves the last update date and time for the "GpiClientModule" from the EngineSyncStatus table.
    /// </summary>
    /// <returns>
    /// The last update date and time as a nullable DateTime value. If no record is found or the value is not available, null is returned.
    /// </returns>
    private DateTime? getLastUpdDateTimeGpi()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Retrieves the last creation date and time for the GPI client module from the database.
    /// </summary>
    /// <returns>The last creation date and time as a nullable DateTime object.</returns>
    private DateTime? getLastCreDateTimeGpi()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .Select(ess => ess.LastCreate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Retrieves the last update date and time for the HubSpotClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the HubSpotClientModule, or null if no record is found.</returns>
    private DateTime? getLastUpdDateTimeHubSpot()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("HubSpotClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }
    
    /// <summary>
    /// Retrieves the last update date and time for the MkgtShipperClientModule from the EngineSyncStatus table.
    /// </summary>
    /// <returns>The last update date and time for the MkgtShipperClientModule</returns>
    private DateTime? getLastUpdDateTimeMkgtShipper()
    {
        return _context.EngineSyncStatus.AsNoTracking()
            .Where(ess => ess.ModuleName.Equals("MkgtShipperClientModule"))
            .Select(ess => ess.LastUpdate)
            .FirstOrDefault();
    }

    /// <summary>
    /// Sets the last create date time for GpiClientModule.
    /// </summary>
    /// <param name="prmDateTime">The date time to be set as last create date time.</param>
    private void setLastCreDateTimeGpi(DateTime prmDateTime)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault()
            .LastCreate = prmDateTime;
        _context.SaveChanges();
    }

    /// <summary>
    /// Sets the last update date and time for the GpiEuropeClientModule in the EngineSyncStatus table.
    /// </summary>
    /// <param name="prmValue">The date and time value to set as the last update time.</param>
    private void setLastUpdDateTimeGpi(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Set the last update date and time for the HubSpotClientModule.
    /// </summary>
    /// <param name="prmValue">The date and time to set as the last update.</param>
    private void setLastUpdDateTimeHubSpot(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("HubSpotClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Set the last update date and time for the MkgtShipperClientModule.
    /// </summary>
    /// <param name="prmValue">The date and time value to set as the last update time.</param>
    private void setLastUpdDateTimeMkgtShipper(DateTime prmValue)
    {
        _context.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientModule"))
            .FirstOrDefault()
            .LastUpdate = prmValue;
        _context.SaveChanges();
    }

    /// <summary>
    /// Retrieves a list of updated entities for the specified module name.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>A list of updated entities.</returns>
    public IList<EtablissementClient> GetUpdatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtClientModule":
                return GetUpdatedEntitiesMkgt();
            
            case "OdooClientModule":
                return GetUpdatedEntitiesOdoo();
            
            case "GpiClientModule":
                return GetUpdatedEntitiesGpi();
            
            case "HubSpotClientModule":
                return GetUpdatedEntitiesHubSpot();
            
            case "DashDocClientModule":
                return GetUpdatedEntitiesDashDoc();
            
            case "MkgtShipperClientModule":
                return GetUpdatedEntitiesMkgtShipper();
            
            default:
                return null;
        }
    }

    /// <summary>
    /// Retrieves the list of created entities for a given module.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <returns>The list of created entities as instances of EtablissementClientExDto.</returns>
    public IList<EtablissementClient> GetCreatedEntities(string moduleName)
    {
        switch (moduleName)
        {
            case "MkgtClientModule":
                return GetCreatedEntitiesMkgt();
            
            case "GpiClientModule":
                return GetCreatedEntitiesGpi();
            
            case "MkgtShipperClientModule":
                return GetCreatedEntitiesMkgtShipper();

            default:
                return null;
        }
    }
    
    public async Task<string> GetMontantGarantieForEntreprise(string siren)
    {
        var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";
        
        var entrepriseBase = await _context.EntrepriseBase.AsNoTracking()
            .Include(e => e.EntrepriseCouverture)
            .Include(e => e.EntrepriseNDCover)
            .SingleOrDefaultAsync(e => e.Siren == siren);

        if (entrepriseBase?.EntrepriseCouverture != null && entrepriseBase.EntrepriseCouverture.MontantGarantie > 0)
        {
            var coverAmount = entrepriseBase.EntrepriseCouverture.MontantGarantie;
            string formatedCoverAmount = coverAmount.ToString("#,0", nfi);
            return $"{formatedCoverAmount} €";
        }
        else if (entrepriseBase?.EntrepriseNDCover != null)
        {
            switch (entrepriseBase.EntrepriseNDCover.Statut)
            {
                case "Garantie totale":
                    var ndCoverAmount = int.Parse(_config.GetSection("CoverPolicies:NDCoverFrance:CoverAmount").Value);
                    string formatedNdCoverAmount = ndCoverAmount.ToString("#,0", nfi);
                    return $"{formatedNdCoverAmount} €";
                case "Pas de garantie":
                    return "0 €";
                default:
                    return "0 €";
            }
        }

        return "0 €";
    }
    
    /// <summary>
    /// Callback method for creating destination IDs for EtablissementClient entities.
    /// </summary>
    /// <param name="moduleName">The name of the module.</param>
    /// <param name="items">The list of EtablissementClient entities.</param>
    /// <returns>The list of EtablissementClient entities with updated destination IDs.</returns>
    public IList<EtablissementClient> CallBackDestIdCreation(string moduleName, IList<EtablissementClient> items)
    {
        switch (moduleName)
        {
            case "MkgtClientModule":
                throw new NotSupportedException();
            case "OdooClientModule":
                throw new NotSupportedException();
            case "GpiClientModule":
                throw new NotSupportedException();
            case "HubSpotClientModule":
                throw new NotSupportedException();
            case "DashDocClientModule":
                throw new NotSupportedException();
            case "MkgtShipperClientModule":
                return updateShipperIds(items);
            default:
                return null;
        }
    }
    
    /// <summary>
    /// Updates the Shipper IDs for the EtablissementClient entities.
    /// </summary>
    /// <param name="items">The list of EtablissementClient entities.</param>
    /// <returns>The list of EtablissementClient entities with updated Shipper IDs.</returns>
    private IList<EtablissementClient> updateShipperIds(IList<EtablissementClient> items)
    {
        foreach (var item in items)
        {
            var etablissementClient = _context.EtablissementClient
                .SingleOrDefault(ec => ec.Siret == item.Siret);

            if (etablissementClient != null)
            {
                etablissementClient.IdShipperDashdoc = item.IdShipperDashdoc;
                _context.EtablissementClient.Update(etablissementClient);
            }
        }

        _context.SaveChanges();
        return items;
    }
}