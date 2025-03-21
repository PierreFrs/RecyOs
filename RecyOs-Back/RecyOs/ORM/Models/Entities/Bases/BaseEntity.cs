/* BaseEntity.cs - Entitée de base commune à toutes les entitées
 * ==============================================================
 * Fichier crée le : 22 janvier 2021
 * Dernière modification le : 
 * Crée par : Benjamin ROLLIN 
 * 
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecyOs.ORM.Entities;

/// <summary>
/// Entitée de base commune à toutes les entitées
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Toutes les entitées doivent disposer d'un ID clé primaire pour le fonctionnement d'Entity Frameworks
    /// </summary>
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }   
}