//  PappersController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.Service.pappers;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("pappers")]
public class PappersController: BaseApiController
{
    private readonly PappersUtilitiesService _pappersService;
    
    public PappersController(PappersUtilitiesService pappersService)
    {
        _pappersService = pappersService;
    }
    
    /// <summary>
    /// Met à jour les données d'une entreprise à partir de son numéro SIRET
    /// </summary>
    /// <param name="siret">Le numéro SIRET de l'entreprise</param>
    [SwaggerResponse(200, "Ok")]
    [HttpPut]
    [Route("update/{siret}")]
    [Authorize(Policy = "Operator")]
    public async Task<IActionResult> UpdateEntreprise(string siret)
    {
        await _pappersService.UpdateEtablissementClientBySiret(siret);
        return Ok();
    }
}