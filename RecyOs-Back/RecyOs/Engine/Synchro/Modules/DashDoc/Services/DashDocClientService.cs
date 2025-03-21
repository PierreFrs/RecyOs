using Microsoft.Extensions.Logging;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.Engine.Modules.DashDoc.Services;

public class DashDocClientService: DashDocBaseService<DashdocCompanyDto>, IDashDocClientService
{
    private readonly ITransportDashdocService _dashdocService;
    private readonly ILogger<DashDocClientService> _logger;
    
    public DashDocClientService(ITransportDashdocService prmDashdocService, ILogger<DashDocClientService> logger ) : base()
    {
        _dashdocService = prmDashdocService;
        _logger = logger;
    }
    
    public override DashdocCompanyDto AddItem(DashdocCompanyDto item)
    {
        _logger.LogTrace("AddItem called with item: {0}", item.Name);
        var res = _dashdocService.CreateDashdocCompanyAsync(item);
        if (res.Result != null)
        {
            _logger.LogTrace("Item added with PK: {0}", res.Result.PK);
        }
        return res.Result;
    }
    
    public override DashdocCompanyDto UpdateItem(DashdocCompanyDto item)
    {
        _logger.LogTrace("UpdateItem called with item: {0}", item.PK);
        var res = _dashdocService.UpdateDashdocCompanyAsync(item);
        if (res.Result != null)
        {
            return res.Result;
        }
        else
        {
            _logger.LogError("UpdateItem failed with item: {0}", item.PK);
            return null;
        }
    }
}