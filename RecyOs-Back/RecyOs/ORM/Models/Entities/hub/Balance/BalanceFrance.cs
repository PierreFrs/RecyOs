// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceFrance.cs
// Created : 2024/02/26 - 11:31
// Updated : 2024/02/26 - 11:31

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.Entities.hub;

public class BalanceFrance : Balance, IClientBalance
{
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Entities.hub.EtablissementClient.BalanceFrances))]
    [Required]
    public virtual EtablissementClient EtablissementClient { get; set; }
    public int ClientId { get; set; }
    
    [ForeignKey(nameof(SocieteId))]
    [InverseProperty(nameof(Entities.hub.Societe.BalanceFrances))]
    [Required]
    public virtual Societe Societe { get; set; }
    public int SocieteId { get; set; }
}