using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RecyOs.OdooImporter.Interfaces;
using RecyOs.OdooImporter.ORM.DbContext;
using RecyOs.OdooImporter.Services;
using ILogger = NLog.ILogger;
using NLog;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Repository;
using RecyOs.OdooImporter.Repositories;


namespace RecyOs.OdooImporter;

public static class Program
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                _logger.Debug("init main");
                services.AddTransient<IAccountAccountRepository<AccountAccountOdooModel>, AccountAccountRepository>();
                services.AddSingleton<IAcountAccountService, AccountAccountService<AccountAccountOdooModel>>();
                services.AddScoped<IRecyOsOdooClient>(provider => new RecyOsOdooClient(
                    baseUrl: "https://odoo.recygroup.fr",
                    db: "modoolar-sipco-main-13225826",
                    username: "b.rollin@recygroup.fr",
                    password: "5510e150e1262a0fc3bf1c52e298407280b30632"
                ));
                services.AddSingleton<IOdooImporterDbContext, OdooImporterDbContext>();
                services.AddSingleton<IAccountMoveLineRepository, AccountMoveLineRepository>();
                services.AddSingleton<ISocieteRepository, SocieteRepository>();
                services.AddSingleton<IClientRepository, ClientRepository>();
                services.AddSingleton<OdooAccountMoveLinesService>();
                services.AddSingleton<IBalanceFranceRepository, BalanceFranceRepository>();
                services.AddSingleton<IBalanceEuropeRepository, BalanceEuropeRepository>();
                services.AddSingleton<IBalanceParticulierRepository, BalanceParticulierRepository>();
                services.AddHostedService<OdooImporterService>(); // Remplacez par votre service
                // Ajoutez d'autres services nécessaires
            });
}

