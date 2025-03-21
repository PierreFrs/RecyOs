// DeletableEntityWithoutId.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 13/08/2024
// Fichier Modifié le : 13/08/2024
// Code développé pour le projet : RecyOs

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

public class DeletableEntityWithoutId
{
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    
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