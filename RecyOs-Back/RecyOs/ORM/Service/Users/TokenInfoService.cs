// TokenInfoService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class TokenInfoService : ITokenInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }

    public int GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext.User.GetUserId();
    }
}