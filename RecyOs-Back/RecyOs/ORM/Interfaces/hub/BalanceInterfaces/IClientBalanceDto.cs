// IClientBalanceDto.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

#nullable enable

public interface IClientBalanceDto
{
    int Id { get; set; }
    #nullable enable
    string? IdOdoo { get; }
    string? Nom { get; set; }
    #nullable disable
}

#nullable disable