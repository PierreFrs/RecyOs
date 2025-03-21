using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.Engine.Modules;

public class DashDocEuropeClientModule : BaseModule<ClientEuropeDto, DashdocCompanyDto>
{
    public DashDocEuropeClientModule(IDashDocClientService prmDataService, IEngineEuropeClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<DashDocEuropeClientModule> logger) : 
        base("DashDocEuropeClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}