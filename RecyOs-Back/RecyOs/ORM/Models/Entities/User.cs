 /** User.cs - Définition du modèle utilisateur
  * ======================================================================0
  * Crée par : Benjamin
  * Fichier Crée le : 22/01/2021
  * Fichier Modifié le : 22/01/2021
  * Code développé pour le projet : ArchimedeServer
  */
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
 using System.Text.Json.Serialization;
 using RecyOs.ORM.Entities.hub;

 namespace RecyOs.ORM.Entities;

 [Table("USER")]
public class User : DeletableEntity
{

    [Column("FIRST_NAME")]
    public string FirstName { get; set; }
    
    [Column("LAST_NAME")]
    public string LastName { get; set; }
    
    [Column("USER_NAME")]
    [Required(ErrorMessage = "Le nom de l'utilisateur est requis")]
    public string UserName { get; set; }
    
    [Column("EMAIL")]
    [Required(ErrorMessage = "L'adresse email est requise")]
    [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide")]
    public string Email { get; set; }
    
    [Column("AVATAR")]
    public string Avatar { get; set; }

    [JsonIgnore]
    [Column("PASSWORD")]
    public string Password { get; set; }
    
    [Column("STATUS")]
    public string Status { get; set; }
    
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserClaim> Claims { get; set; }
    #nullable enable
    [InverseProperty("User")]
    public virtual Commercial? Commercial { get; set; }


    [Column("Societe_Id")]
    public int? SocieteId { get; set; }
    public virtual Societe? Societe { get; set; }
    #nullable disable
}