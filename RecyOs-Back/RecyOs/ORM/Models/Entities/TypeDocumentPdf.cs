// TypeDocumentPdf.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 04/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("TypeDocumentPdf")]
public class TypeDocumentPdf : TrackedEntity
{
    [Required]
    [Column("Label")]
    [StringLength(50)]
    public string Label { get; set; }
}