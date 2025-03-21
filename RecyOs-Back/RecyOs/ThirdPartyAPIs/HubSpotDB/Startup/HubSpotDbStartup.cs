// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotDbStartup.cs
// Created : 2024/04/16 - 13:51
// Updated : 2024/04/16 - 13:51

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.HubSpotDB.Entities;
using RecyOs.HubSpotDB.Interfaces;
using RecyOs.HubSpotDB.Repository;
using RecyOs.HubSpotDB.Services;
using RecyOs.ORM.Service;
using RecyOs.ORM.Startup;

namespace RecyOs.HubSpotDB.Startup;

public class HubSpotDbStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICompaniesService, CompaniesService<Companies>>();
        services.AddTransient<ICompaniesRepository<Companies>, CompaniesRepository>();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }
    
    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        HubSpotDbStartup.ConfigureServices(services, null);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        HubSpotDbStartup.Configure(app, env);
    }
}