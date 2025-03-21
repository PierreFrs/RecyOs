// DashdocDbStartup.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.HubSpotDB.Startup;
using RecyOs.ORM.Startup;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;
using RecyOs.ThirdPartyAPIs.DashdocDB.Repository;
using RecyOs.ThirdPartyAPIs.DashdocDB.Service;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Startup;

public class DashdocDbStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITransportDashdocSettings, TransportDashdocSettings>();
        services.AddTransient<ITransportDashdocService, TransportDashdocService>();
        services.AddTransient<ITransportDashdocRepository, TransportDashdocRepository>();
        services.AddTransient<IShipperDashdocSettings, ShipperDashdocSettings>();
        services.AddTransient<IShipperDashdocService, ShipperDashdocService>();
        services.AddTransient<IShipperDashdocRepository, ShipperDashdocRepository>();
    }
    
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }
    
    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        DashdocDbStartup.ConfigureServices(services, null);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        DashdocDbStartup.Configure(app, env);
    }
}