/** NotificationDto.cs - Définition du DTO Notification
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 03/02/2025
 * Fichier Modifié le : 03/02/2025
 * Code développé pour le projet : Fuse Framework
 */

using System;

namespace RecyOs.ORM.DTO;

public class NotificationDto
{
    public int Id { get; set; }
    public string NotificationId { get; set; }

#nullable enable
    public string? Icon { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
#nullable disable

    public DateTime Time { get; set; }
    public bool UseRouter { get; set; }
    public bool Read { get; set; }
    public UserDto User { get; set; }
    public int UserId { get; set; }
}
