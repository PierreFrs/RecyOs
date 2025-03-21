/** Flux.cs - Définition du modèle FuseNavigationItem
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 22/05/2023
 * Fichier Modifié le : 22/05/2023
 * Code développé pour le projet : Fuse Framework
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("FuseNavigationItem")]
public class FuseNavigationItem : DeletableEntity
{
    [Column("MenuId")]
    [Required]
    public int MenuId { get; set; }
    
    [Column("MenuPosition")]
    [Required]
    public int MenuPosition { get; set; }
    
    [Column("Type")]
    [StringLength(50)]
    public string Type { get; set; }
    
#nullable enable
    [Column("Title")]
    [StringLength(50)]
    public string? Title { get; set; }
    
    [Column("Subtitle")]
    [StringLength(50)]
    public string? Subtitle { get; set; }

    [Column("Hidden")]
    public bool? Hidden { get; set; }
    
    [Column("Active")]
    public bool? Active { get; set; }
    
    [Column("Disabled")]
    public bool? Disabled { get; set; }
    
    [Column("Tooltip")]
    [StringLength(50)]
    public string? Tooltip { get; set; }
    
    [Column("Link")]
    [StringLength(50)]
    public string? Link { get; set; }
    
    [Column("Fragment")]
    [StringLength(50)]
    public string? Fragment { get; set; }
    
    [Column("PreserveFragment")]
    public bool? PreserveFragment { get; set; }
    
    [Column("BadgeId")]
    public int? BadgeId { get; set; }
    
    [Column("Badge")]
    [ForeignKey(nameof(BadgeId))]
    public virtual FuseNavigationItemBadge? Badge { get; set; }
    
    [Column("ExternalLink")]
    public bool? ExternalLink { get; set; }
    
    [Column("Target")]
    [StringLength(50)]
    public string? Target { get; set; }
    
    [Column("ExactMatch ")]
    public bool? ExactMatch { get; set; }
    
    [Column("Icon")]
    [StringLength(50)]
    public string? Icon { get; set; }
    
    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(FuseNavigationItem.Children))]
    public virtual FuseNavigationItem? Parent { get; set; }
    
    public int? ParentId { get; set; }
    
    public virtual ICollection<FuseNavigationItem>? Children { get; set; }
    
#nullable disable
    
    [ForeignKey(nameof(RoleId))]
    [Required]
    public virtual Role Role { get; set; }
    
    public int RoleId { get; set; }
}