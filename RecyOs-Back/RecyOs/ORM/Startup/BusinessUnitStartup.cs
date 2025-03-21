// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BusinessUnitStartup.cs
// Created : 2024/01/22 - 09:31
// Updated : 2024/01/22 - 09:31

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class BusinessUnitStartup
{
    public static void ConfigureBusinessUnit(IServiceCollection services)
    {
        services.AddTransient<IBusinessUnitService<BusinessUnitDto>, BusinessUnitService<BusinessUnit, BusinessUnitDto>>();
        services.AddTransient<IBusinessUnitRepository<BusinessUnit>, BusinessUnitRepository>();
        
        services.AddTransient<IEtablissementClientBusinessUnitService<EtablissementClientBusinessUnitDto, BusinessUnitDto>, EtablissementClientBusinessUnitService<EtablissementClientBusinessUnit, EtablissementClientBusinessUnitDto, BusinessUnit, BusinessUnitDto>>();
        services.AddTransient<IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit>, EtablissementClientBusinessUnitRepository>();
        
        services.AddTransient<IClientEuropeBusinessUnitService<ClientEuropeBusinessUnitDto, BusinessUnitDto>, ClientEuropeBusinessUnitService<ClientEuropeBusinessUnit, ClientEuropeBusinessUnitDto, BusinessUnit, BusinessUnitDto>>();
        services.AddTransient<IClientEuropeBusinessUnitRepository<ClientEuropeBusinessUnit, BusinessUnit>, ClientEuropeBusinessUnitRepository>();
    }
}