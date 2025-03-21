using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.Controllers.ClientsControllers.BalanceClientControllers;

[Route("balances-particuliers")]
public class BalanceParticuliersControler : BaseApiController
{
    private readonly IBalanceParticulierService _balanceParticulierService;
    public BalanceParticuliersControler(IBalanceParticulierService balanceParticulierService)
    {
        _balanceParticulierService = balanceParticulierService;
    }
    
    /// GET for Grid
    /// <summary>
    /// Permet d'obtenir une liste d'objets BalanceParticulier pour une grille
    /// </summary>
    /// <returns>Liste d'objets BalanceParticulier</returns>
    [HttpGet]
    [Route("grid")]
    public async Task<IActionResult> GetDataForGrid([FromQuery] BalanceParticulierGridFilter filter)
    {
        var balances = await _balanceParticulierService.GetDataForGrid(filter);
        return Ok(balances);
    }
    
}
