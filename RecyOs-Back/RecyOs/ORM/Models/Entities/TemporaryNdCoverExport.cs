// TemporaryNdCoverExport.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 03/03/2025
// Fichier Modifié le : 03/03/2025
// Code développé pour le projet : RecyOs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("TemporaryNdCoverExport")]
public class TemporaryNdCoverExport : TrackedEntity
{
    [Column("file_row")]
    [Required]
    public int FileRow { get; set; }

    [Column("siren")]
    [Required]
    public string Siren { get; set; }
}