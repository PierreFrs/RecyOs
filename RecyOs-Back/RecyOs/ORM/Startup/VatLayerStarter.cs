using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Service.vatlayer;

namespace RecyOs.ORM.Startup;

public class VatLayerStarter : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<VatlayerApiService>();
        services.AddTransient<IVatlayerUtilitiesService, VatlayerUtilitiesService>();
    }
    
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Ajoutez ici la configuration spécifique de PappersStartup
    }
    
    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        VatLayerStarter.ConfigureServices(services, configuration);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        VatLayerStarter.Configure(app, env);
    }
}