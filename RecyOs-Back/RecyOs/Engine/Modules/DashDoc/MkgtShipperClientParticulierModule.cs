using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.DashDoc.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.Engine.Modules;

public class MkgtShipperClientParticulierModule: BaseModule<ClientParticulierDto, DashdocShipperDto>
{
    public MkgtShipperClientParticulierModule(IDashdocShipperService prmDataService, IEngineClientParticulierService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtShipperClientParticulierModule> logger) : 
        base("MkgtShipperClientParticulierModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}