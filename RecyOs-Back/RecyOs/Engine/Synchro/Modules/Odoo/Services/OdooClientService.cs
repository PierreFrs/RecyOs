//  OdooClientService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB;
using NLog;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Interfaces;


namespace RecyOs.Engine.Modules.Odoo.Services;

public class OdooClientService: OdooBaseService<ResPartnerDto>, IOdooClientService
{
    private readonly IResPartnerService _resPartnerService;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public OdooClientService(IResPartnerService prmResPartnerService) : base()
    {
        _resPartnerService = prmResPartnerService;
    }
    
    public override ResPartnerDto AddItem(ResPartnerDto item)
    {
        Logger.Trace("AddItem called with item: {0}", item.Name);
        var res = _resPartnerService.InsertPartnerAsync(item);
        return res.Result;
    }
    
    public override ResPartnerDto UpdateItem(ResPartnerDto item)
    {
        Logger.Trace("UpdateItem called with item: {0}", item.Name);
        var res = _resPartnerService.UpdatePartnerAsync(item);
        if (res.Result != null)
        {
            return res.Result;
        }
        else
        {
            Logger.Error("UpdateItem failed with item: {0}", item.Name);
            return null;
        }
    }
    
    
}