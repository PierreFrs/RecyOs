/** TrackedEntity.cs - Définition du modèle article
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 21/03/2023
 * Fichier Modifié le : 21/03/2023
 * Code développé pour le projet : RecOS.TrackedEntity
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

public class TrackedEntity : DeletableEntity
{
#nullable enable
    [Column("created_date")]
    [DefaultValue("GETDATE()")]
    public DateTime? CreateDate { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    
    [Column("created_by")]
    [MaxLength(55)]
    [DefaultValue("SUSER_SNAME()")]
    public string? CreatedBy { get; set; }

    [Column("updated_by")]
    [MaxLength(55)]
    public string? UpdatedBy { get; set; }
    
    #nullable disable
}