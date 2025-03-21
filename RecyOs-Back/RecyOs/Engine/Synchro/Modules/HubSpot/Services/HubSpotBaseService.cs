// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotBaseService.cs
// Created : 2024/04/17 - 09:21
// Updated : 2024/04/17 - 09:21

using RecyOs.Engine.Services;

namespace RecyOs.Engine.Modules.HubSpot.Services;

public abstract class HubSpotBaseService<TDestination> : BaseDataService<TDestination> where TDestination : class
{
    protected HubSpotBaseService() : base()
    {
        
    }
}