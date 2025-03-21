using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.EFCore.Repository.Application;
using RecyOs.ORM.EFCore.Repository.gpiSync;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.dashboard;
using RecyOs.ORM.Interfaces.gpiSync;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.IParameters;
using RecyOs.ORM.Service;
using RecyOs.ORM.Service.dashboard;
using RecyOs.ORM.Service.gpiSync;
using RecyOs.ORM.Service.hub;

namespace RecyOs.ORM.Startup;

public class OrmStartup : IBaseStartup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEntrepriseBaseService, EntrepriseBaseService<EntrepriseBase>>();
        services.AddTransient<IEntrepriseBaseRepository<EntrepriseBase>, EntrepriseBaseRepository>();

        services.AddTransient<IEtablissementFicheService, EtablissementFicheService<EtablissementFiche>>();
        services.AddTransient<IEtablissementFicheRepository<EtablissementFiche>, EtablissementFicheRepository>();

        services.AddTransient<IEtablissementClientService, EtablissementClientService<EtablissementClient>>();
        services.AddTransient<IEtablissementClientRepository<EtablissementClient>, EtablissementClientRepository>();
        
        services.AddTransient<IEtablissementFournisseurService, EtablissementFournisseurService<EtablissementClient>>();
        services.AddTransient<IEtablissementFournisseurRepository<EtablissementClient>, EtablissementFournisseurRepository>();
        
        services.AddTransient<IClientEuropeService, ClientEuropeService<ClientEurope>>();
        services.AddTransient<IClientEuropeRepository<ClientEurope>, ClientEuropeRepository>();
        
        services.AddTransient<IFournisseurEuropeService, FournisseurEuropeService<ClientEurope>>();
        services.AddTransient<IFournisseurEuropeRepository<ClientEurope>, FournisseurEuropeRepository>();
        
        services.AddTransient<IClientParticulierService, ClientParticulierService>();
        services.AddTransient<IClientParticulierRepository, ClientParticulierRepository>();
        
        services.AddTransient<IEntrepriseCouvertureService, EntrepriseCouvertureService<EntrepriseCouverture>>();
        services.AddTransient<IEntrepriseCouvertureRepository<EntrepriseCouverture>, EntrepriseCouvertureRepository>();
        
        services.AddTransient<IEntrepriseNDCoverService, EntrepriseNDCoverService<EntrepriseNDCover>>();
        services.AddTransient<IEntrepriseNDCoverRepository<EntrepriseNDCover>, EntrepriseNDCoverRepository>();
        
        services.AddTransient<IFuseNavigationMenuService, FuseNavigationMenuService<FuseNavigationItem>>();
        services.AddTransient<IFuseNavigationMenuRepository<FuseNavigationItem>, FuseNavigationMenuRepository>();

        services.AddTransient<IParameterService, ParameterService<Parameter>>();
        services.AddTransient<IParameterRepository<Parameter>, ParameterRepository>();

        services.AddTransient<IDashboardCustomerService, DashboardCustomerService<EtablissementClient, 
            EtablissementFiche, EntrepriseCouverture, EntrepriseNDCover>>();

        services.AddTransient<IGpiSyncEtablissementClientRepository<EtablissementClient>, GpiSyncEtablissementClientRepository>();
        services.AddTransient<IGpiSyncClientEuropeRepository<ClientEurope>, GpiSyncClientEuropeRepository>();

        services.AddTransient<ICounterRepository<Counter>, CounterRepository>();
        services.AddTransient<ICounterService, CounterService<Counter>>();

        services.AddTransient<INotificationRepository<Notification>, NotificationRepository>();
        services.AddTransient<INotificationService, NotificationService<Notification>>();
        
        services.AddTransient<IGpiSyncService, GpiSyncService>();

        services.AddTransient<ITokenInfoService, TokenInfoService>();

        services.AddTransient<IGroupRepository, GroupRepository>();
        services.AddTransient<IGroupService, GroupService>();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Do nothing because no need to configure anything but interface is required
    }

    void IBaseStartup.ConfigureServices(IServiceCollection services)
    {
        OrmStartup.ConfigureServices(services, null);
    }
    
    void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        OrmStartup.Configure(app, env);
    }

}