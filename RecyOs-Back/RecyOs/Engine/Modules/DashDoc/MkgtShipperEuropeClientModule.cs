using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.Engine.Modules;

public class MkgtShipperEuropeClientModule : BaseModule<ClientEuropeDto, DashdocShipperDto>
{
    public MkgtShipperEuropeClientModule(IDashdocShipperService prmDataService, IEngineEuropeClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtShipperEuropeClientModule> logger) : 
        base("MkgtShipperEuropeClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}