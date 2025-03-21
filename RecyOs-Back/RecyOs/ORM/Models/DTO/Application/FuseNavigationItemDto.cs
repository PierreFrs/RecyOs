/** Flux.cs - Définition du modèle FuseNavigationItem
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 23/05/2023
 * Fichier Modifié le : 23/05/2023
 * Code développé pour le projet : Fuse Framework
 */

using System.Collections.Generic;

namespace RecyOs.ORM.DTO;

public class FuseNavigationItemDto
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public int MenuPosition { get; set; }
    public string Type { get; set; }
#nullable enable
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public bool? Hidden { get; set; }
    public bool? Active { get; set; }
    public bool? Disabled { get; set; }
    public string? Tooltip { get; set; }
    public string? Link { get; set; }
    public string? Fragment { get; set; }
    public bool? PreserveFragment { get; set; }
    public FuseNavigationItemBadgeDto? Badge { get; set; }
    public bool? ExternalLink { get; set; }
    public string? Target { get; set; }
    public bool? ExactMatch { get; set; }
    public string? Icon { get; set; }
    public ICollection<FuseNavigationItemDto>? Children { get; set; }
#nullable disable
    public RoleDto Role { get; set; }
}