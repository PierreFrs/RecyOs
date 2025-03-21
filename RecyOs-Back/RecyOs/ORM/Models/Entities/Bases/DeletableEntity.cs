// /** DeletableEntity.cs - Entité de base commune à toutes les entitées dont les entitées sont supprimable
//  * ==================================================================================================================
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : ArchimedeServer
//  */

using System.ComponentModel;

namespace RecyOs.ORM.Entities;

public class DeletableEntity :BaseEntity
{
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}