// Created by : Pierre FRAISSE
// RecyOs => RecyOs => Societe.cs
// Created : 2024/02/23 - 15:11
// Updated : 2024/02/23 - 15:11

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class Societe : TrackedEntity
{
    [Column("Nom")]
    [Required]
    public string Nom { get; set; }
    [Column("IdOdoo")]
    [Required]
    public string IdOdoo { get; set; }
    
    public virtual ICollection<BalanceFrance> BalanceFrances { get; set; }
    public virtual ICollection<BalanceEurope> BalanceEuropes { get; set; }
    public virtual ICollection<BalanceParticulier> BalanceParticuliers { get; set; }
    public virtual ICollection<User> Users { get; set; }
}