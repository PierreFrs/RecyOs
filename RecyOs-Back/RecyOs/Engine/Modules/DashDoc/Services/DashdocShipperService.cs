using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.Engine.Modules.DashDoc.Services;

public class DashdocShipperService: DashDocBaseService<DashdocShipperDto>, IDashdocShipperService
{
    private readonly IShipperDashdocService _dashdocService;
    private readonly ILogger<DashdocShipperService> _logger;
    private readonly IMapper _mapper;
    
    public DashdocShipperService(IShipperDashdocService prmDashdocService, ILogger<DashdocShipperService> logger, IMapper mapper ) : base()
    {
        _dashdocService = prmDashdocService;
        _logger = logger;
        _mapper = mapper;
    }
    
    public override DashdocShipperDto AddItem(DashdocShipperDto item)
    {
        _logger.LogTrace("AddItem called with item: {0}", item.Name);
        var shipper = _mapper.Map<DashdocShipperDto, DashdocCompanyDto>(item);
        var res = _dashdocService.CreateDashdocCompanyAsync(shipper);
        if (res.Result != null)
        {
            _logger.LogTrace("Item added with PK: {0}", res.Result.PK);
        }
        var ret = _mapper.Map<DashdocCompanyDto, DashdocShipperDto>(res.Result);
        return ret;
    }
    
    public override DashdocShipperDto UpdateItem(DashdocShipperDto item)
    {
        var shipper= _mapper.Map<DashdocShipperDto, DashdocCompanyDto>(item);
        var res = _dashdocService.UpdateDashdocCompanyAsync(shipper);
        if (res.Result != null)
        {
            var ret = _mapper.Map<DashdocCompanyDto, DashdocShipperDto>(res.Result);
            return ret;
        }
        else
        {
            _logger.LogError("UpdateItem failed with item: {0}", item.PK);
            return null;
        }
    }
    
}