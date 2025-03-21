// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BaseHubSpotRepository.cs
// Created : 2024/04/16 - 14:08
// Updated : 2024/04/16 - 14:08

using Microsoft.Extensions.Configuration;

namespace RecyOs.HubSpotDB.Repository;

public class BaseHubSpotRepository
{
    protected readonly string _apiKey;
    protected readonly string _endPoint;
    public BaseHubSpotRepository(IConfiguration configuration)
    {
        _apiKey = configuration.GetSection("HubSpot:ApiKey").Value;
        _endPoint = configuration.GetSection("HubSpot:EndPoint").Value;
    }
}