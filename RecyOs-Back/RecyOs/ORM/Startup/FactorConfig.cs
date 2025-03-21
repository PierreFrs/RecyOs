// <copyright file="FactorConfig.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.EFCore.Repository.hub.FactorClientBuRepositories;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using RecyOs.ORM.Service.hub;

namespace RecyOs.ORM.Startup;

public class FactorConfig
{
    public static void ConfigureFactorConfig(IServiceCollection services)
    {
        services.AddTransient(typeof(IBaseFactorClientBuService<,>), typeof(BaseFactorClientBuService<,>));
        services.AddTransient(typeof(IBaseFactorClientBuRepository<>), typeof(BaseFactorClientBuRepository<>));
        
        services.AddTransient<IFactorClientFranceBuService, FactorClientFranceBuService>();
        services.AddTransient<IFactorClientFranceBuRepository, FactorClientFranceBuRepository>();
        
        services.AddTransient<IFactorClientEuropeBuService, FactorClientEuropeBuService>();
        services.AddTransient<IFactorClientEuropeBuRepository, FactorClientEuropeBuRepository>();
    }   
}