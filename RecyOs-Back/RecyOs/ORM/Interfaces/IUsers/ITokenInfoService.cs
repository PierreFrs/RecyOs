// ITokenInfoService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

namespace RecyOs.ORM.Interfaces;

public interface ITokenInfoService
{
    public string GetCurrentUserName();
    public int GetCurrentUserId();
}