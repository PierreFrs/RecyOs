// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EtablissementClientBusinessUnit.cs
// Created : 2024/01/23 - 17:18
// Updated : 2024/01/23 - 17:18

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class ClientEuropeBusinessUnit : DeletableEntityWithoutId
{
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Entities.hub.ClientEurope.ClientEuropeBusinessUnits))]
    [Required]
    public virtual ClientEurope ClientEurope { get; set; }
    public int ClientId { get; set; }
    
    [ForeignKey(nameof(BusinessUnitId))]
    [InverseProperty(nameof(Entities.hub.BusinessUnit.ClientEuropeBusinessUnits))]
    [Required]
    public virtual BusinessUnit BusinessUnit { get; set; }
    public int BusinessUnitId { get; set; }
}