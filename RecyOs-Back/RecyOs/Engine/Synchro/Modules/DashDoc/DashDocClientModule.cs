using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.Engine.Modules;

public class DashDocClientModule : BaseModule<EtablissementClientDto, DashdocCompanyDto>
{
    public DashDocClientModule(IDashDocClientService prmDataService, IEngineEtablissementClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<DashDocClientModule> logger) : 
        base("DashDocClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}