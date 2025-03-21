// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BusinessUnit.cs
// Created : 2024/01/19 - 10:46
// Updated : 2024/01/19 - 10:46

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

[Table("business_unit")]
public class BusinessUnit : TrackedEntity
{
    [Required]
    [Column("libelle")]
    [StringLength(50)]
    public string Libelle { get; set; }
    
    public virtual ICollection<EtablissementClientBusinessUnit> EtablissementClientBusinessUnits { get; set; }
    
    public virtual ICollection<ClientEuropeBusinessUnit> ClientEuropeBusinessUnits { get; set; }
    
    public virtual ICollection<FactorClientFranceBu> FactorClientFranceBus { get; set; }
    
    public virtual ICollection<FactorClientEuropeBu> FactorClientEuropeBus { get; set; }
}