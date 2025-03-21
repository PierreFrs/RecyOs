// /** CurrentContextProvider.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */
using Microsoft.AspNetCore.Http;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class CurrentContextProvider : ICurrentContextProvider
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentContextProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public ContextSession GetCurrentContext()
    {
        if (_accessor.HttpContext.User != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
        {
            var currentUserId = _accessor.HttpContext.User.GetUserId();

            if (currentUserId > 0)
            {
                return new ContextSession { UserId = currentUserId };
            }
        }
        return null;
    }
}