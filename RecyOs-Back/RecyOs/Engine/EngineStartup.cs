using Microsoft.Extensions.DependencyInjection;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Startup;
using RecyOs.Engine.Alerts;
using RecyOs.Engine.Tools;

namespace RecyOs.Engine;

public static class EngineStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDataContextEngine, DataContextEngine>();
        ToolsStartup.ConfigureServices(services);                // Module Tools
        SynchroStartup.ConfigureServices(services);              // Module Synchro   
        AlertsStartup.ConfigureServices(services);               // Module Alerts
    }
}