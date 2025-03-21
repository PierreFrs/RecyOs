// /** BaseStartup.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 24/01/2021
//  * Fichier Modifié le : 24/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace RecyOs.ORM.Startup;

public interface IBaseStartup
{
    void ConfigureServices(IServiceCollection services);
    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
}