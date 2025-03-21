// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialStartup.cs
// Created : 2024/03/26 - 15:18
// Updated : 2024/03/26 - 15:18

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public class CommercialStartup
{
    protected CommercialStartup() { }

    public static void ConfigureCommercial(IServiceCollection services)
    {
        services.AddTransient<ICommercialBaseRepository, CommercialBaseRepository>();
        services.AddTransient<ICommercialBaseService, CommercialBaseService>();
    }
}