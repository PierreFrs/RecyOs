// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotClientModule.cs
// Created : 2024/04/17 - 09:47
// Updated : 2024/04/17 - 09:47

using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.HubSpot.Interfaces;
using RecyOs.HubSpotDB.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class HubSpotClientModule : BaseModule<EtablissementClientDto, CompaniesDto>
{
    public HubSpotClientModule(IHubSpotClientService prmDataService, IEngineEtablissementClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<HubSpotClientModule> logger) : 
        base("HubSpotClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}