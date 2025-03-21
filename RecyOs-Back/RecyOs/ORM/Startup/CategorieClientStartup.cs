// CategorieClientStartup.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using Microsoft.Extensions.DependencyInjection;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.Startup;

public static class CategorieClientStartup
{
    public static void ConfigureCategorieClient(IServiceCollection services)
    {
        services.AddTransient<ICategorieClientRepository<CategorieClient>, CategorieClientRepository>();

        services
            .AddTransient<ICategorieClientService<CategorieClientDto>,
                CategorieClientService<CategorieClient, CategorieClientDto>>();
    }
}