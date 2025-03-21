// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotClientService.cs
// Created : 2024/04/17 - 09:23
// Updated : 2024/04/17 - 09:23

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Modules.HubSpot.Interfaces;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Entities;
using RecyOs.HubSpotDB.Interfaces;

namespace RecyOs.Engine.Modules.HubSpot.Services;

public class HubSpotClientService: HubSpotBaseService<CompaniesDto>, IHubSpotClientService
{
    private readonly ICompaniesService _companiesService;
    private readonly ILogger<HubSpotClientService> _logger;
    public HubSpotClientService(ICompaniesService prmCompaniesService, ILogger<HubSpotClientService> logger ) : base()
    {
        _companiesService = prmCompaniesService;
        _logger = logger;
    }
    
    public override CompaniesDto AddItem(CompaniesDto item)
    {
        _logger.LogTrace("AddItem called with item: {0}", item.Id);
        var res = _companiesService.CreateCompany(item);
        return res.Result;
    }

    public override CompaniesDto UpdateItem(CompaniesDto item)
    {
        _logger.LogTrace("UpdateItem called with item: {0}", item.Id);
        var res = _companiesService.UpdateCompany(item);
        if (res.Result != null)
        {
            return res.Result;
        }
        else
        {
            _logger.LogError("UpdateItem failed with item: {0}", item.Id);
            return null;
        }
    }
}