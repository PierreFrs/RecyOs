//  BaseApiController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 30/01/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace RecyOs.Controllers;

[ApiController]
[Authorize]
[EnableCors("_myAllowSpecificOrigins")]
public abstract class BaseApiController : ControllerBase
{
        
}