//  DashboardControler.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 10/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.dashboard;
using RecyOs.ORM.Interfaces.dashboard;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("dashboard")]
public class DashboardControler: BaseApiController
{
    
    private readonly IDashboardCustomerService _dashboardCustomerService;
    
    public DashboardControler(IDashboardCustomerService dashboardCustomerService)
    {
        _dashboardCustomerService = dashboardCustomerService;
    }
    
    /// <summary>
    /// retourne les données du dashboard client
    /// </summary>
    /// <returns>Objet CustomerDashboartDto</returns>
    [SwaggerResponse(200, "Ok", typeof(DashboardCustomerDto))]
    [HttpGet]
    [Route("customer")]
    [Authorize(Policy = "DashboardClient")]
    public async Task<IActionResult> GetDashboardCustomer()
    {
        var dashboard = await _dashboardCustomerService.GetDashboard();
        return Ok(dashboard);
    }
}