/** Notification.cs - Définition du modèle Notification
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 03/02/2025
 * Fichier Modifié le : 03/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("Notification")]
public class Notification : DeletableEntity
{
    
#nullable enable
    [Column("Icon")]
    [StringLength(50)]
    public string? Icon { get; set; }

    [Column("Image")]
    [StringLength(255)]
    public string? Image { get; set; }

    [Column("Title")]
    [StringLength(100)]
    public string? Title { get; set; }

    [Column("Description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("Link")]
    [StringLength(255)]
    public string? Link { get; set; }
#nullable disable

    [Column("Time")]
    [Required]
    public DateTime Time { get; set; }

    [Column("UseRouter")]
    public bool UseRouter { get; set; }

    [Column("Read")]
    [Required]
    public bool Read { get; set; }

    [ForeignKey(nameof(UserId))]
    [Required]
    public virtual User User { get; set; }

    public int UserId { get; set; }
}
