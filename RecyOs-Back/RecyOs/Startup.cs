using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RecyOs.Commands.Startup;
using RecyOs.Engine;
using RecyOs.Engine.Startup;
using RecyOs.Helpers;
using RecyOs.HubSpotDB.Startup;
using RecyOs.MKGT_DB.Startup;
using RecyOs.OdooDB.Startup;
using RecyOs.ORM.Startup;
using NLog;
using RecyOs.Engine.Services;
using RecyOs.ORM.Interfaces;
using RecyOs.ThirdPartyAPIs.DashdocDB.Startup;
using ILogger = NLog.ILogger;

namespace RecyOs
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        private static string GetXmlCommentsPath()
        {
            var app = System.AppContext.BaseDirectory;
            return System.IO.Path.Combine(app, "RecyOs.xml");
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMigrationStatusService, MigrationStatusService>();

            // Récupère la configuration CORS à partir d'appsettings.json
            var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
            services.AddDbContext<DataContext>();                                  
            IdentityStartup.ConfigureServices(services, Configuration);            // Module Identity
            OrmStartup.ConfigureServices(services, Configuration);                 // Module ORM
            PappersStartup.ConfigureServices(services, Configuration);             // Module Pappers
            VatLayerStarter.ConfigureServices(services, Configuration);            // Module VatLayer
            CommandsStartup.ConfigureServices(services, Configuration);            // Module Commands
            UploadFilesStartup.ConfigureFilesUpload(services);                     // Module Upload
            CategorieClientStartup.ConfigureCategorieClient(services);             // Categories Clients
            BusinessUnitStartup.ConfigureBusinessUnit(services);                   // Business Unit
            SocieteStartup.ConfigureSociete(services);                             // Societe
            BalanceStartup.ConfigureBalanceStartup(services);                      // Balance
            CommercialStartup.ConfigureCommercial(services);                       // Commercial
            ValidationConfig.ConfigureValidationServices(services);                // Data Validation
            EmailDomainStartup.ConfigureEmailDomainStartup(services);              // Email Domain
            DateFormaterStartup.ConfigureDateFormaterStartup(services);            // Date Formater
            FactorConfig.ConfigureFactorConfig(services);                          // Factor Config
            FactorExportStartup.ConfigureFactorExportStartup(services);            // Factor Export
            EngineStartup.ConfigureServices(services);                            // Engine
            
            // Third party API Startups
            MkgtDbStartup.ConfigureServices(services, Configuration);              // Module MKGT_DB
            OdooDbStartup.ConfigureServices(services, Configuration);              // Module OdooDB
            HubSpotDbStartup.ConfigureServices(services, Configuration);           // Module HubSpotDB
            DashdocDbStartup.ConfigureServices(services, Configuration);           // Module DashdocDB
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecyOs Server", Version = "v0.3" });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                        new OpenApiSecurityScheme  
                        {  
                            Reference = new OpenApiReference  
                            {  
                                Type = ReferenceType.SecurityScheme,  
                                Id = "Bearer"  
                            }  
                        },  
                        new string[] {}  
  
                    }
                });
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.EnableAnnotations();
                c.IgnoreObsoleteActions();
            });
            
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IMigrationStatusService migrationStatus)
        {
            _logger.Trace("Starting Configure method." );
            // Appliquer automatiquement les migrations au démarrage de l'application
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>(); // Remplace DataContext par le nom de ton DbContext
                    context.Database.Migrate();
                    migrationStatus.SetMigrationCompleted();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "An error occurred while migrating the database.");
                }
            }
            
            if (env.IsDevelopment())
            {
                _logger.Warn("Development environment detected. Enabling Swagger.");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecyOs v0.3"));
            }

            app.UseHttpsRedirection();
            
            IdentityStartup.Configure(app, env);
            OrmStartup.Configure(app, env);
            PappersStartup.Configure(app, env);
            VatLayerStarter.Configure(app, env);
            MkgtDbStartup.Configure(app, env);
            CommandsStartup.Configure(app, env);
            OdooDbStartup.Configure(app, env);

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}