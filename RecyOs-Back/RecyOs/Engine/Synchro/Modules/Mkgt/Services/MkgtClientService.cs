//  MkgtClientService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using Microsoft.Extensions.Logging;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.MKGT_DB.Interfaces;
using NLog;

namespace RecyOs.Engine.Modules.Mkgt.Services;

public class MkgtClientService: MkgtBaseService<EtablissementMkgtDto>, IMkgtClientService
{
    private readonly IFCliService _fCliService;
    private readonly ILogger<MkgtClientService> _logger;
    public MkgtClientService(IFCliService prmFCliService, ILogger<MkgtClientService> logger ) : base()
    {
        _fCliService = prmFCliService;
        _logger = logger;
    }
    
    public override EtablissementMkgtDto AddItem(EtablissementMkgtDto item)
    {
        _logger.LogTrace("AddItem called with item: {0}", item.code);
        var res = _fCliService.InsertEtablissementClient(item);
        return res.Result;
    }

    public override EtablissementMkgtDto UpdateItem(EtablissementMkgtDto item)
    {
        _logger.LogTrace("UpdateItem called with item: {0}", item.code);
        var res = _fCliService.UpdateEtablissementClient(item);
        if (res.Result != null)
        {
            return res.Result;
        }
        else
        {
            _logger.LogError("UpdateItem failed with item: {0}", item.code);
            return null;
        }
    }
}