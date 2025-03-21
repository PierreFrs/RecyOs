using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.Engine.Modules;

public class OdooClientParticulierModule : BaseModule<ClientParticulierDto, ResPartnerDto>
{
    public OdooClientParticulierModule(IOdooClientService prmDataService, IEngineClientParticulierService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<OdooClientParticulierModule> logger) : 
        base("OdooClientParticulierModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}