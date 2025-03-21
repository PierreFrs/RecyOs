// CategorieClient.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Routing.Matching;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Entities;

[Table("CategorieClient")]
public class CategorieClient : TrackedEntity
{
    [Required]
    [Column("Label")]
    [StringLength(50)]
    public string CategorieLabel { get; set; }
    
    public virtual ICollection<EtablissementClient> EtablissementClients { get; set; }
    
    public virtual ICollection<ClientEurope> ClientEuropes { get; set; }
}