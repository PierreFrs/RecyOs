using AutoMapper;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Service;

public class TransportDashdocService : DashdocService, ITransportDashdocService
{
    public TransportDashdocService(ITransportDashdocRepository dashdocRepository,
        IMapper mapper) : base(dashdocRepository, mapper)
    {
        
    }
}