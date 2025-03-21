// BalanceParticulierService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.Entities.hub;

public class BalanceParticulier : Balance, IClientBalance
{
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Entities.hub.ClientParticulier.BalanceParticuliers))]
    [Required]
    public virtual ClientParticulier ClientParticuliers { get; set; }
    public int ClientId { get; set; }

    [ForeignKey(nameof(SocieteId))]
    [InverseProperty(nameof(Entities.hub.Societe.BalanceParticuliers))]
    [Required]
    public virtual Societe Societe { get; set; }
    public int SocieteId { get; set; }
}