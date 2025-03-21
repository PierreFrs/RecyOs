// /** UserRole.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

public class UserRole
{
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(Entities.User.UserRoles))]
    [Required]
    public virtual User User { get; set; }
    public int UserId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    [InverseProperty(nameof(Entities.Role.UserRoles))]
    [Required]
    public virtual Role Role { get; set; }
    public int RoleId { get; set; }
}