// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EtablissementClientBusinessUnit.cs
// Created : 2024/01/23 - 17:18
// Updated : 2024/01/23 - 17:18

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class EtablissementClientBusinessUnit : DeletableEntityWithoutId
{
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Entities.hub.EtablissementClient.EtablissementClientBusinessUnits))]
    [Required]
    public virtual EtablissementClient EtablissementClient { get; set; }
    public int ClientId { get; set; }
    
    [ForeignKey(nameof(BusinessUnitId))]
    [InverseProperty(nameof(Entities.hub.BusinessUnit.EtablissementClientBusinessUnits))]
    [Required]
    public virtual BusinessUnit BusinessUnit { get; set; }
    public int BusinessUnitId { get; set; }
}