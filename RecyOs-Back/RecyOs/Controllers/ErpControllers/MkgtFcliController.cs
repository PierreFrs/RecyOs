//  MkgtFcliController.cs -
// ======================================================================
// Crée par : Benjamin
// Fichier Crée le : 14/09/2023
// Fichier Modifié le : 14/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.MKGT_DB.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("mkgt-fcli")]
public class MkgtFcliController : BaseApiController
{
    private readonly IFCliService _mkgtFcliService;
    
    public MkgtFcliController(IFCliService mkgtFcliService)
    {
        _mkgtFcliService = mkgtFcliService;
    }
    
    /// <summary>
    /// Permet d'obtenir un client à partir de son code client
    /// </summary>
    /// <param name="codeClient">Le code client du client</param>
    /// <returns>Objet Fcli</returns>
    [SwaggerResponse(200, "Ok", typeof(EtablissementMkgtDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client non trouvé")]
    [HttpGet]
    [Route("{codeClient}")]
    public async Task<IActionResult> Get(string codeClient)
    {
        var client = await _mkgtFcliService.GetClient(codeClient);
        if (client == null) return NotFound();
        return Ok(client);
    }
}