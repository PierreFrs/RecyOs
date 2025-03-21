// Group.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 10/02/2025
// Fichier Modifié le : 10/02/2025
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class Group : TrackedEntity
{
    [Column("name")]
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public virtual ICollection<ClientEurope> ClientEuropes { get; set; }
    public virtual ICollection<EtablissementClient> EtablissementClients { get; set; }

}