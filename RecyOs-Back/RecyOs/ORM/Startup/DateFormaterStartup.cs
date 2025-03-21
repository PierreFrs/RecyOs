// Created by : Pierre FRAISSE
// RecyOs => RecyOs => DateFormaterStartup.cs
// Created : 2024/04/22 - 11:58
// Updated : 2024/04/22 - 11:58

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class DateFormaterStartup
{
    public static void ConfigureDateFormaterStartup(IServiceCollection services)
    {
        services.AddTransient<IDateFormater, DateFormater>();
    }
}