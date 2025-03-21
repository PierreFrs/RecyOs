// /** UserClaim.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.ComponentModel.DataAnnotations;

namespace RecyOs.ORM.Entities;

public class UserClaim : BaseEntity
{
    public int UserId { get; set; }

    public virtual User User { get; set; }
    
    [Required]
    public string ClaimType { get; set; }
   
    [Required]
    public string ClaimValue { get; set; }
}