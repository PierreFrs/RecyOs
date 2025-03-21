using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.MKGT_DB.Repository;
using RecyOs.MKGT_DB.Services;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Startup;

namespace RecyOs.MKGT_DB.Startup;

public class MkgtDbStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<IFCliService, FCliService<Fcli>>();
        services.AddTransient<IFCliRepository<Fcli>, FcliRepository>();

    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }

    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        MkgtDbStartup.ConfigureServices(services, null);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        MkgtDbStartup.Configure(app, env);
    }
}
