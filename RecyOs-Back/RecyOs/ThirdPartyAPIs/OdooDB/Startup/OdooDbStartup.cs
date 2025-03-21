using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Engine.Services;
using RecyOs.OdooDB;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Repository;
using RecyOs.OdooDB.Resolvers;
using RecyOs.OdooDB.Services;
using RecyOs.ORM.Startup;

namespace RecyOs.OdooDB.Startup;

public class OdooDbStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IResPartnerService, ResPartnerService<ResPartnerOdooModel>>();
        services.AddTransient<IResPartnerRepository<ResPartnerOdooModel>, ResPartnerRepository>();
        
        services.AddTransient<IIrPropertyRepository<IrPropertyOdooModel>, IrPropertyRepository>();
        services.AddTransient<IResCountryRepository<ResCountryOdooModel>, ResCountryRepository>();
        services.AddTransient<IBaseOdooRepository, BaseOdooRepository>();
        services.AddTransient<IBaseOdooService, BaseOdooService>();
        services.AddTransient<IMoveAccountRepository, MoveAccountRepository>();
        services.AddTransient<IMoveAccountService, MoveAccountService>();
        services.AddTransient<CountryNameToIdResolver>();
        services.AddTransient<IResCompanyRepository<ResCompanyOdooModel>, ResCompanyRepository>();
        services.AddSingleton<IResCompanyService, ResCompanyService<ResCompanyOdooModel>>();
        services.AddTransient<IAccountAccountRepository<AccountAccountOdooModel>, AccountAccountRepository>();
        services.AddSingleton<IAcountAccountService, AccountAccountService<AccountAccountOdooModel>>();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }

    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        OdooDbStartup.ConfigureServices(services, null);
    }

    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        OdooDbStartup.Configure(app, env);
    }
}