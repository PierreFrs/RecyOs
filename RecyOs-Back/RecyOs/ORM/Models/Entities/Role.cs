// /** Role.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */
using System.Collections.Generic;

namespace RecyOs.ORM.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
        
    public virtual ICollection<UserRole> UserRoles { get; set; }
}