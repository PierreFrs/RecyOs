// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotEuropeClientModule.cs
// Created : 2024/04/17 - 09:48
// Updated : 2024/04/17 - 09:48

using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.HubSpot.Interfaces;
using RecyOs.HubSpotDB.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class HubSpotEuropeClientModule : BaseModule<ClientEuropeDto, CompaniesDto>
{
    public HubSpotEuropeClientModule(IHubSpotClientService prmDataService, IEngineEuropeClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<HubSpotEuropeClientModule> logger) : 
        base("HubSpotEuropeClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}