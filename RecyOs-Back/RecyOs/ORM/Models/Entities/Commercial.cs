// Created by : Pierre FRAISSE
// RecyOs => RecyOs => Commercial.cs
// Created : 2024/03/26 - 14:35
// Updated : 2024/03/26 - 14:35

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Entities;

[Table("Commercial")]

public class Commercial: TrackedEntity
{
    [Required]
    [Column("Firstname")]
    [StringLength(50)]
    public string Firstname { get; set; }
    
    [Required]
    [Column("Lastname")]
    [StringLength(50)]
    public string Lastname { get; set; }
    
    [Required]
    [Column("Username")]
    [StringLength(50)]
    public string Username { get; set; }
    
    [Required]
    [Column("Phone")]
    [StringLength(30)]
    public string Phone { get; set; }
    
    [Required]
    [Column("Email")]
    [StringLength(50)]
    public string Email { get; set; }
    
    [Required]
    [Column("CodeMkgt")]
    [StringLength(2)]
    public string CodeMkgt { get; set; }
    
    #nullable enable
    [Column("Id_HubSpot")]
    public string? IdHubSpot { get; set; }

    [Column("User_Id")]
    [ForeignKey("User")]
    public int? UserId { get; set; }
    
    [InverseProperty("Commercial")]
    public virtual User? User { get; set; }

    #nullable disable
    
    public virtual ICollection<EtablissementClient> EtablissementClients { get; set; }
    public virtual ICollection<ClientEurope> ClientEuropes { get; set; }
}