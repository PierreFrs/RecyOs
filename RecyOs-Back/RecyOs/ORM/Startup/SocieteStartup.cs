// Created by : Pierre FRAISSE
// RecyOs => RecyOs => SocieteStartup.cs
// Created : 2024/02/23 - 15:56
// Updated : 2024/02/23 - 15:56

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public class SocieteStartup
{
    public static void ConfigureSociete(IServiceCollection services)
    {
        services.AddTransient<ISocieteBaseRepository, SocieteBaseRepository>();

        services
            .AddTransient<ISocieteBaseService, SocieteBaseService>();
    }
}