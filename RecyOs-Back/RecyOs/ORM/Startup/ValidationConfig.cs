// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ValidationConfig.cs
// Created : 2024/03/26 - 16:57
// Updated : 2024/03/26 - 16:57

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public class ValidationConfig
{
    protected ValidationConfig() { }
    public static void ConfigureValidationServices(IServiceCollection services)
    {
        services.AddScoped<IDataValidationService, DataValidationService>();
    }
}