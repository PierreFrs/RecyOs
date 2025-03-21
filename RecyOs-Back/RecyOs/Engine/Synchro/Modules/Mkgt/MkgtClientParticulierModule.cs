using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.Engine.Modules;

public class MkgtClientParticulierModule : BaseModule<ClientParticulierDto, EtablissementMkgtDto>
{
    public MkgtClientParticulierModule(IMkgtClientService prmDataService, IEngineClientParticulierService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtClientParticulierModule> logger) : 
        base("MkgtClientParticulierModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}