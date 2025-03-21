using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class OdooEuropeClientModule: BaseModule<ClientEuropeDto, ResPartnerDto>
{
    public OdooEuropeClientModule(IOdooClientService prmdataService, IEngineEuropeClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<OdooEuropeClientModule> logger) : 
        base("OdooEuropeClientModule", prmdataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}