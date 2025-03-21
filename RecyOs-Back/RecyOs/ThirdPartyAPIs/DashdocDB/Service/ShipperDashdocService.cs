using AutoMapper;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Service;

public class ShipperDashdocService : DashdocService, IShipperDashdocService
{
    public ShipperDashdocService(IShipperDashdocRepository dashdocRepository,
        IMapper mapper) : base(dashdocRepository, mapper)
    {
        
    }
}