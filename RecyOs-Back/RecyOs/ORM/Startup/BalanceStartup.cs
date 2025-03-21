// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceStartup.cs
// Created : 2024/02/26 - 13:32
// Updated : 2024/02/26 - 13:32

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Service.hub;
using RecyOs.ORM.Service.hub.BalanceServices;

namespace RecyOs.ORM.Startup;

public static class BalanceStartup
{
    public static void ConfigureBalanceStartup(IServiceCollection services)
    {
        services.AddTransient<IBalanceFranceRepository, BalanceFranceRepository>();
        services.AddTransient<IBalanceFranceService, BalanceFranceService>();
        
        services.AddTransient<IBalanceEuropeRepository, BalanceEuropeRepository>();
        services.AddTransient<IBalanceEuropeService, BalanceEuropeService>();

        services.AddTransient<IBalanceParticulierRepository, BalanceParticulierRepository>();
        services.AddTransient<IBalanceParticulierService, BalanceParticulierService>();
        services.AddTransient<IBalanceRepository<BalanceParticulier>, BalanceParticulierRepository>();

    }
}