// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>FactorExportStartup.cs
// Created : 2024/05/2121 - 14:05

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class FactorExportStartup
{
    public static void ConfigureFactorExportStartup(IServiceCollection services)
    {
        services.AddTransient<IFactorFileExportService, FactorFileExportService>();
        services.AddTransient<IFactorFileExportRepository, FactorFileExportRepository>();
    }
}