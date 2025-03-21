using Microsoft.Extensions.DependencyInjection;
using RecyOs.Engine.Tools.Interfaces;
using RecyOs.Engine.Tools;

namespace RecyOs.Engine.Tools;

public static class ToolsStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
    }
}
