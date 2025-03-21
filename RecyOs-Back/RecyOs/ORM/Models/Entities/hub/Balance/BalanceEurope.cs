// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceEurope.cs
// Created : 2024/02/26 - 14:18
// Updated : 2024/02/26 - 14:18

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.Entities.hub;

public class BalanceEurope : Balance, IClientBalance
{
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Entities.hub.ClientEurope.BalanceEuropes))]
    [Required]
    public virtual ClientEurope ClientEurope { get; set; }
    public int ClientId { get; set; }
    
    [ForeignKey(nameof(SocieteId))]
    [InverseProperty(nameof(Entities.hub.Societe.BalanceEuropes))]
    [Required]
    public virtual Societe Societe { get; set; }
    public int SocieteId { get; set; }
}