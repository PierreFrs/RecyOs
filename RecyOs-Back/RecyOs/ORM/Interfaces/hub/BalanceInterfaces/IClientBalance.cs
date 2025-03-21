// IClientBalance.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 29/08/2024
// Fichier Modifié le : 29/08/2024
// Code développé pour le projet : RecyOs

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IClientBalance
{
    int ClientId { get; set; }
    int SocieteId { get; set; }
}