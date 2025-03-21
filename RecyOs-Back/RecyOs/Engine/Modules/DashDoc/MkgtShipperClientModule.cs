using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.Engine.Modules;

public class MkgtShipperClientModule: BaseModule<EtablissementClientDto, DashdocShipperDto>
{
    public MkgtShipperClientModule(IDashdocShipperService prmDataService, IEngineEtablissementClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtShipperClientModule> logger) : 
        base("MkgtShipperClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}