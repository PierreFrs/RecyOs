// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EmailDomainStartup.cs
// Created : 2024/04/19 - 16:38
// Updated : 2024/04/19 - 16:38

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class EmailDomainStartup
{
    public static void ConfigureEmailDomainStartup(IServiceCollection services)
    {
        services.AddTransient<IEmailDomainService, EmailDomainService>();
    }
}