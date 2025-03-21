using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("counters")]
public class CounterController : BaseApiController
{
    private readonly ICounterService _counterService;
    
    public CounterController(ICounterService counterService)
    {
        _counterService = counterService;
    }

    /// <summary>
    /// Retrieves the counter with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the counter to retrieve.</param>
    /// <returns>The counter with the specified ID.</returns>
    [SwaggerResponse(200, "Retourne le compteur", typeof(CounterDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Compteur non trouvé")]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var counter = await _counterService.GetCounterById(id);
        if (counter == null)
        {
            return NotFound();
        }
        return Ok(counter);
    }

    /// <summary>
    /// Retrieves the counter with the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [SwaggerResponse(200, "Retourne le compteur", typeof(CounterDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Compteur non trouvé")]
    [HttpGet]
    [Route("get_by_name/{name}")]
    public async Task<IActionResult> Get(string name)
    {
        var counter = await _counterService.GetCounterByName(name);
        if (counter == null)
        {
            return NotFound();
        }
        return Ok(counter);
    }

    /// <summary>
    /// Increments the value of the counter with the specified name.
    /// </summary>
    /// <param name="name">The name of the counter to increment.</param>
    /// <returns>The updated counter.</returns>
    /// <response code="200">Returns the incremented counter.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Web service error.</response>
    [SwaggerResponse(200, "Retourne le compteur incrémenté", typeof(CounterDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(500, "Erreur webService")]
    [HttpGet]
    [Route("increment/{name}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Increment(string name)
    {
        var counter = await _counterService.IncrementCounterByName(name);
        if (counter == null)
        {
            return NotFound();
        }
        return Ok(counter);
    }
    
    
}