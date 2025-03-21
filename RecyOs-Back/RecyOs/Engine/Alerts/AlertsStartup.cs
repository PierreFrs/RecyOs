/// <summary>
/// Classe de démarrage pour les alertes.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using Microsoft.Extensions.DependencyInjection;
using RecyOs.Engine.Alerts.Interfaces;
using RecyOs.Engine.Alerts.Repositories;
using RecyOs.Engine.Alerts.Services;

namespace RecyOs.Engine.Alerts;

public static class AlertsStartup
{
    /// <summary>
    /// Configure les services pour les alertes.
    /// </summary>
    /// <param name="services">Collection de services.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IEngineMessageMailRepository, EngineMessageMailRepository>();
        services.AddSingleton<IEngineMessageMailService, EngineMessageMailService>();
        services.AddAutoMapper(typeof(AlertsStartup).Assembly);
        services.AddHostedService<EmailBackgroundService>();
    }
}
