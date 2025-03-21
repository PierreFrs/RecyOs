using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.HubSpotDB.Interfaces;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;
using RecyOs.ORM.Service.pappers;

namespace RecyOs.ORM.Startup
{
    public class PappersStartup : IBaseStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Ajoutez ici votre clé d'API de Pappers
            var pappersApiKey = configuration.GetValue<string>("Pappers:ApiKey");

            // Configurer HttpClient pour Pappers API
            services.AddHttpClient("PappersClient", client =>
            {
                client.BaseAddress = new Uri("https://api.pappers.fr/v2/");
                client.DefaultRequestHeaders.Add("api_token", pappersApiKey);
            });

            // Enregistrer le service PappersAPIService
            services.AddTransient<IPappersApiService, PappersApiService>();

            // Use factory injection to break circular dependency
            services.AddTransient<IEtablissementClientService, EtablissementClientService<EtablissementClient>>();
            services.AddTransient<IEtablissementFournisseurService, EtablissementFournisseurService<EtablissementClient>>();
            
            services.AddScoped<IEtablissementRepository<EtablissementClient>, EtablissementClientRepository>();
            services.AddScoped<IEtablissementFicheRepository<EtablissementFiche>, EtablissementFicheRepository>();
            
            services.AddTransient<IEtablissementServiceUtilitaryMethods, EtablissementServiceUtilitaryMethods>();

            services.AddTransient<IPappersUtilitiesService>(provider => 
            {
                Func<IEtablissementClientService> etablissementClientServiceFactory = 
                    () => provider.GetRequiredService<IEtablissementClientService>();
                Func<IEtablissementFournisseurService> etablissementFournisseurServiceFactory = 
                    () => provider.GetRequiredService<IEtablissementFournisseurService>();
                return new PappersUtilitiesService(
                    provider.GetRequiredService<IPappersApiService>(),
                    etablissementClientServiceFactory,
                    etablissementFournisseurServiceFactory,
                    provider.GetRequiredService<IEntrepriseBaseService>(),
                    provider.GetRequiredService<IEtablissementFicheService>(),
                    provider.GetRequiredService<ITokenInfoService>(),
                    provider.GetRequiredService<ILogger<PappersUtilitiesService>>()
                    );
            });

            // Enregistrement du mapping (si nécessaire)
            RegisterMapping(services);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ajoutez ici la configuration spécifique de PappersStartup
        }

        void IBaseStartup.ConfigureServices(IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            PappersStartup.ConfigureServices(services, configuration);
        }

        void IBaseStartup.Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            PappersStartup.Configure(app, env);
        }

        private static void RegisterMapping(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PappersStartup));
        }
    }
}
